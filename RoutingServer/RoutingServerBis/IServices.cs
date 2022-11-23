using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RoutingServer
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        Itinerary GetBestPath(Location startLocation, Location endLocation);
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "RoutingServer.ContractType".

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
    }

    [DataContract]
    public class Itinerary
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public List<Feature> features { get; set; }
        [DataMember]
        public List<double> bbox { get; set; }
        [DataMember]
        public Metadata metadata { get; set; }
    }

    [DataContract]
    public class Engine
    {
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public DateTime build_date { get; set; }
        [DataMember]
        public DateTime graph_date { get; set; }
    }

    [DataContract]
    public class Feature
    {
        [DataMember]
        public List<double> bbox { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public Properties properties { get; set; }
        [DataMember]
        public Geometry geometry { get; set; }
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
    public class Metadata
    {
        [DataMember]
        public string attribution { get; set; }
        [DataMember]
        public string service { get; set; }
        [DataMember]
        public long timestamp { get; set; }
        [DataMember]
        public Query query { get; set; }
        [DataMember]
        public Engine engine { get; set; }
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
    public class Query
    {
        [DataMember]
        public List<List<double>> coordinates { get; set; }
        [DataMember]
        public string profile { get; set; }
        [DataMember]
        public string format { get; set; }
    }

    [DataContract]
    public class Segment
    {
        [DataMember]
        public double distance { get; set; }
        [DataMember]
        public int duration { get; set; }
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
        public int duration { get; set; }
    }


}
