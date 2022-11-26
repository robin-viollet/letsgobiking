using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using System.Device.Location;

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

        MultipleCheckpointsItinary IServices.GetBestPath(Location startLocation, Location endLocation)
        {
            Console.WriteLine("Request received : ");
            Console.WriteLine("\tStart location : " + startLocation);
            Console.WriteLine("\tEnd location : " + endLocation);

            try
            {
                /*startLocation = new Location("22 Rue Jacques Preiss", "Mulhouse", "France", "68100");
                endLocation = new Location("44 Avenue Roger Salengro", "Mulhouse", "France", "68100");*/

                List<Contract> contracts = this.JCDecaux.GetAllContracts();
                Contract concernedContract = this.GetConcernedContract(startLocation, contracts);

                List<Station> stationOfTheContract = this.JCDecaux.GetAllStations(concernedContract);

                GeoCoordinate startCoordinate = this.GetGeoCoordinate(startLocation);
                Console.WriteLine(startCoordinate.Latitude + " | " + startCoordinate.Longitude);

                GeoCoordinate endCoordinate = this.GetGeoCoordinate(endLocation);
                Console.WriteLine(endCoordinate.Latitude + " | " + endCoordinate.Longitude);

                Station pickUpStation = this.GetClosestPickUpStation(startCoordinate, stationOfTheContract);
                Station dropOffStation = this.GetClosestDropOffStation(endCoordinate, stationOfTheContract);

                Console.WriteLine("Pick up : " + pickUpStation.number + pickUpStation.address);
                Console.WriteLine("Drop off : " + dropOffStation.number + dropOffStation.address);

                List<Double[]> checkpointsCoordinates = new List<double[]>();
                checkpointsCoordinates.Add(new double[] { startCoordinate.Longitude, startCoordinate.Latitude});
                checkpointsCoordinates.Add(new double[] { pickUpStation.position.longitude, pickUpStation.position.latitude});
                checkpointsCoordinates.Add(new double[] { dropOffStation.position.longitude, dropOffStation.position.latitude });
                checkpointsCoordinates.Add(new double[] { endCoordinate.Longitude, endCoordinate.Latitude });

                Console.WriteLine(JsonSerializer.Serialize(checkpointsCoordinates));

                MultipleCheckpointsItinary multipleCheckpointsItinary = this.OpenRouteDirectionService.GetFootWalkingItinerary(checkpointsCoordinates);

                Console.WriteLine(JsonSerializer.Serialize(multipleCheckpointsItinary));

                return multipleCheckpointsItinary;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        private Contract GetConcernedContract(Location location, List<Contract> contracts)
        {
            foreach (Contract contract in contracts)
            {
                if (contract.cities != null && contract.cities.Contains(location.city))
                {
                    return contract;
                }
            }
            //FIXME gestion des exceptions ???
            return null;
        }

        private Station GetClosestPickUpStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            return this.GetClosestStation(geoCoordinate, stations.Where(s => s.CanPickUpABike()).ToList());
        }

        private Station GetClosestDropOffStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            return this.GetClosestStation(geoCoordinate, stations.Where(s => s.CanDropOffABike()).ToList());
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
