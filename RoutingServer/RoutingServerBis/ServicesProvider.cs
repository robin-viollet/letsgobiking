using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using System.Device.Location;
using RoutingServer.IProxyCacheServices;

namespace RoutingServer
{
    public class ServicesProvider : IServices
    {
        private readonly JCDecaux JCDecaux;
        private readonly Nominatim Nominatim;
        private readonly OpenRouteDirectionService OpenRouteDirectionService;

        public ServicesProvider()
        {
            this.JCDecaux = new JCDecaux();
            this.Nominatim = new Nominatim();
            this.OpenRouteDirectionService= new OpenRouteDirectionService();
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
                /*startLocation = new Location("22 Rue Jacques Preiss", "Mulhouse", "France", "68100");
                endLocation = new Location("44 Avenue Roger Salengro", "Mulhouse", "France", "68100");*/

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

                Itinerary straightItineray = this.OpenRouteDirectionService.GetFootWalkingItinerary(
                            this.GetCoordinates(startCoordinate),
                            this.GetCoordinates(endCoordinate));

                if (pickUpStation.number == dropOffStation.number)
                {
                    requestResult.itineraries = new Itinerary[] {straightItineray};
                    return requestResult;
                }

                Itinerary itineraryToPickUpStation = this.OpenRouteDirectionService.GetFootWalkingItinerary(
                            this.GetCoordinates(startCoordinate),
                            this.GetCoordinates(pickUpStation)
                            );

                Itinerary bicycleItinerary = this.OpenRouteDirectionService.GetBikingItinerary(
                            this.GetCoordinates(pickUpStation),
                            this.GetCoordinates(dropOffStation)
                            );

                Itinerary itineraryToArrivalPoint = this.OpenRouteDirectionService.GetFootWalkingItinerary(
                            this.GetCoordinates(dropOffStation),
                            this.GetCoordinates(endCoordinate)
                            );

                double walkingDuration = itineraryToPickUpStation.GetTotalDuration() + 
                    itineraryToArrivalPoint.GetTotalDuration();

                if(walkingDuration > straightItineray.GetTotalDuration())
                {
                    requestResult.itineraries = new Itinerary[] { straightItineray };
                    return requestResult;
                }

                requestResult.itineraries = new Itinerary[]
                {
                    itineraryToPickUpStation,
                    bicycleItinerary,
                    itineraryToArrivalPoint
                };

                return requestResult;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        private String GetCoordinates(GeoCoordinate geoCoordinate)
        {
            return geoCoordinate.Longitude + "," + geoCoordinate.Latitude;
        }

        private String GetCoordinates(Station station)
        {
            return station.position.longitude + "," + station.position.latitude;
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

        public ContractTypes.Station GetStation()
        {
            throw new NotImplementedException();
        }
    }
}
