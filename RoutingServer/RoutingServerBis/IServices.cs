using RoutingServer.IProxyCacheServices;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web.Routing;

namespace RoutingServer
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        RequestResult GetBestPath(Location startLocation, Location endLocation);

        [OperationContract]
        List<Contract> GetAllContracts();
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "RoutingServer.ContractType".

    [DataContract]
    public class RequestResult
    {
        [DataMember]
        public String errorMessage { get; set; }

        [DataMember]
        public Itinerary[] itineraries { get; set; }

        [DataMember]
        public String instructionQueueId { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember]
        public String street { get; set; }
        [DataMember]
        public String city { get; set; }
        [DataMember]
        public String country { get; set; }
        [DataMember]
        public String postalCode { get; set; }

        public Location(String street, String city, String country, String postalCode)
        {
            this.street = street;
            this.city = city;
            this.country = country;
            this.postalCode = postalCode;
        }

        public override String ToString()
        {
            return this.street + " | " + this.city + " | " + this.country + " | " + this.postalCode; 
        }
    }

    [DataContract]
    public class Itinerary
    {
        [DataMember]
        public List<Route> routes { get; set; }
        [DataMember]
        public List<double> bbox { get; set; }

        public double GetTotalDuration()
        {
            double totalDuration = 0;
            foreach (Route route in routes)
            {
                totalDuration += route.summary.duration;
            }
            return totalDuration;
        }
    }

    [DataContract]
    public class Route
    {
        [DataMember]
        public Summary summary { get; set; }
        [DataMember]
        public List<Segment> segments { get; set; }
        [DataMember]
        public List<double> bbox { get; set; }
        [DataMember]
        public string geometry { get; set; }
        [DataMember]
        public List<int> way_points { get; set; }
    }

    [DataContract]
    public class Geometry
    {
        [DataMember]
        public List<List<double>> coordinates { get; set; }
        [DataMember]
        public string type { get; set; }
    }

    [DataContract]
    public class Properties
    {
        [DataMember]
        public List<Segment> segments { get; set; }
        [DataMember]
        public Summary summary { get; set; }
        [DataMember]
        public List<int> way_points { get; set; }
    }

    [DataContract]
    public class Segment
    {
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public List<Step> steps { get; set; }
    }

    [DataContract]
    public class Step
    {
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public double duration { get; set; }
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string instruction { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<int> way_points { get; set; }
    }

    [DataContract]
    public class Summary
    {
        [DataMember]
        public double distance { get; set; }
        [DataMember] 
        public double duration { get; set; }
    }

}
