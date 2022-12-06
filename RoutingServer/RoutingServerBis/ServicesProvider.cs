using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Device.Location;
using RoutingServer.IProxyCacheServices;
using Apache.NMS.ActiveMQ;
using Apache.NMS;

namespace RoutingServer
{
    public class ServicesProvider : IServices
    {
        private static readonly NumberFormatInfo numberFormatInfo = new NumberFormatInfo();

        static ServicesProvider()
        {
            numberFormatInfo.NumberDecimalSeparator = ".";
        }

        private int queueId = 0;
        private ISession session;

        private readonly JCDecaux JCDecaux;
        private readonly Nominatim Nominatim;
        private readonly OpenRouteDirectionService OpenRouteDirectionService;

        public ServicesProvider()
        {
            this.JCDecaux = new JCDecaux();
            this.Nominatim = new Nominatim();
            this.OpenRouteDirectionService= new OpenRouteDirectionService();

            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

            // Create a single Connection from the Connection Factory.
            IConnection connection = connectionFactory.CreateConnection();
            connection.Start();

            // Create a session from the Connection.
            this.session = connection.CreateSession();
        }

        public List<Contract> GetAllContracts()
        {
            return this.JCDecaux.GetAllContracts();
        }

        RequestResult IServices.GetBestPath(Location startLocation, Location endLocation)
        {
            Console.WriteLine("Request received : ");
            Console.WriteLine("\tStart location : " + startLocation);
            Console.WriteLine("\tEnd location : " + endLocation);

            try
            {
                RequestResult requestResult = new RequestResult();

                List<Contract> contracts = this.JCDecaux.GetAllContracts();
                Contract departureContract = this.GetConcernedContract(startLocation, contracts);

                if(departureContract == null)
                {
                    requestResult.errorMessage = "No contract found for the city of departure!";
                    return requestResult;
                }

                Contract arrivalContract = this.GetConcernedContract(endLocation, contracts);
                if (arrivalContract == null)
                {
                    requestResult.errorMessage = "No contract found for the city of arrival!";
                    return requestResult;
                }
                else if(departureContract.name != arrivalContract.name)
                {
                    requestResult.errorMessage = "The departure and arrival cities don't belong to the same contract!";
                    return requestResult;
                }

                List<Station> stationOfTheContract = this.JCDecaux.GetAllStations(departureContract);

                GeoCoordinate startCoordinate = this.GetGeoCoordinate(startLocation);
                Console.WriteLine(startCoordinate.Latitude + " | " + startCoordinate.Longitude);

                GeoCoordinate endCoordinate = this.GetGeoCoordinate(endLocation);
                Console.WriteLine(endCoordinate.Latitude + " | " + endCoordinate.Longitude);

                Station pickUpStation = this.GetClosestPickUpStation(startCoordinate, stationOfTheContract);
                Station dropOffStation = this.GetClosestDropOffStation(endCoordinate, stationOfTheContract);

                Console.WriteLine("Pick up : " + pickUpStation.number + pickUpStation.address);
                Console.WriteLine("Drop off : " + dropOffStation.number + dropOffStation.address);

                Itinerary[] bestItinerary = this.GetBestItinerary(startCoordinate, endCoordinate,
                                                                    pickUpStation, dropOffStation);

                requestResult.itineraries = bestItinerary;

                requestResult.instructionQueueId = this.CreateQueueWithInstructions(bestItinerary);

                return requestResult;
            }
            catch (CantFindAddressException e)
            {
                Console.WriteLine(e.Message);
                return new RequestResult
                {
                    errorMessage = e.Message
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        private Itinerary[] GetBestItinerary(GeoCoordinate startCoordinate, GeoCoordinate endCoordinate, Station pickUpStation, Station dropOffStation)
        {

            Itinerary straightItineray = this.OpenRouteDirectionService.GetFootWalkingItinerary(
                    new List<Double[]>
                    {
                        this.GetCoordinatesArray(startCoordinate),
                        this.GetCoordinatesArray(endCoordinate)
                    });

            if (pickUpStation.number == dropOffStation.number)
            {
                return new Itinerary[] { straightItineray };
            }

            Itinerary itineraryToPickUpStation = this.OpenRouteDirectionService.GetFootWalkingItinerary(
                    new List<Double[]>
                    {
                        this.GetCoordinatesArray(startCoordinate),
                        this.GetCoordinatesArray(pickUpStation)
                    });

            Itinerary bicycleItinerary = this.OpenRouteDirectionService.GetBikingItinerary(
                    new List<Double[]>
                    {
                        this.GetCoordinatesArray(pickUpStation),
                        this.GetCoordinatesArray(dropOffStation)
                    });

            Itinerary itineraryToArrivalPoint = this.OpenRouteDirectionService.GetFootWalkingItinerary(
                    new List<Double[]>
                    {
                        this.GetCoordinatesArray(dropOffStation),
                        this.GetCoordinatesArray(endCoordinate)
                    });

            double walkingDuration = itineraryToPickUpStation.GetTotalDuration() +
                itineraryToArrivalPoint.GetTotalDuration();

            if (walkingDuration > straightItineray.GetTotalDuration())
            {
                return new Itinerary[] { straightItineray };
            }

            return new Itinerary[]
            {
                    itineraryToPickUpStation,
                    bicycleItinerary,
                    itineraryToArrivalPoint
            };
        }

        private String CreateQueueWithInstructions(Itinerary[] itineraries)
        {
            // Use the session to target a queue.
            IDestination destination = session.GetQueue("" + queueId);

            // Create a Producer targetting the selected queue.
            IMessageProducer producer = session.CreateProducer(destination);
            // You may configure everything to your needs, for instance:
            producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            foreach(Itinerary itinerary in itineraries)
            {
                foreach(Route route in itinerary.routes)
                {
                    foreach(Segment segment in route.segments)
                    {
                        foreach(Step step in segment.steps)
                        {
                            producer.Send(this.session.CreateTextMessage(step.instruction));
                        }
                    }
                }
            }

            producer.Close();

            return "" + (++this.queueId);
        }

        private String GetCoordinatesString(GeoCoordinate geoCoordinate)
        {
            return geoCoordinate.Longitude.ToString(numberFormatInfo) + "," + geoCoordinate.Latitude.ToString(numberFormatInfo);
        }

        private String GetCoordinatesString(Station station)
        {
            return station.position.longitude.ToString(numberFormatInfo) + "," + station.position.latitude.ToString(numberFormatInfo);
        }

        private double[] GetCoordinatesArray(GeoCoordinate geoCoordinate)
        {
            return new double[] { geoCoordinate.Longitude, geoCoordinate.Latitude};
        }

        private double[] GetCoordinatesArray(Station station)
        {
            return new double[] { station.position.longitude, station.position.latitude};
        } 

        private Contract GetConcernedContract(Location location, List<Contract> contracts)
        {
            foreach (Contract contract in contracts)
            {
                if (contract.cities != null)
                {
                    foreach(String city in contract.cities)
                    {
                        if (city.ToLower().Equals(location.city.ToLower()))
                        {
                            return contract;
                        }
                    }
                }
            }
            return null;
        }

        private Station GetClosestPickUpStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            return this.GetClosestStation(geoCoordinate, stations.Where(s => s.canPickUpABike).ToList());
        }

        private Station GetClosestDropOffStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            return this.GetClosestStation(geoCoordinate, stations.Where(s => s.canDropOfABike).ToList());
        }

        private Station GetClosestStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            Station closestStation = null;
            double closestDistance = Double.MaxValue;

            GeoCoordinate toCompare;
            double temporaryDistance;
            foreach (Station station in stations)
            {
                toCompare = new GeoCoordinate(station.position.latitude, station.position.longitude);
                temporaryDistance = geoCoordinate.GetDistanceTo(toCompare);
                if (temporaryDistance <= closestDistance)
                {
                    closestStation = station;
                    closestDistance = temporaryDistance;
                }
            }
            return closestStation;
        }

        private GeoCoordinate GetGeoCoordinate(Location location)
        {
            Place place = this.Nominatim.GetPlace(location);
            return new GeoCoordinate(Double.Parse(place.lat, CultureInfo.InvariantCulture), Double.Parse(place.lon, CultureInfo.InvariantCulture));
        }
    }
}
