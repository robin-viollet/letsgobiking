using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyCache
{
    public class JCDecauxStationsItem : AbstractCacheItem<List<Station>>
    {
        private static readonly String ApiUrl = "https://api.jcdecaux.com/vls/v3/stations?apiKey=ac8828262f441bebc10d8f59f3aa109399f6f66b&contract=";

        public JCDecauxStationsItem(String contract) : base(ApiUrl + contract){
            Console.WriteLine("Send a request to obtain stations from the contract : " + contract + ".");
            foreach(Station s in base.GetItemContent()){
                s.SetCanPickUpABike();
                s.SetCanDropOffABike();
            }
        }
    }

    [DataContract]
    public class Station
    {
        [DataMember]
        public int number { get; set; }
        [DataMember]
        public string contractName { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public Position position { get; set; }
        [DataMember]
        public bool banking { get; set; }
        [DataMember]
        public bool bonus { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public DateTime lastUpdate { get; set; }
        [DataMember]
        public bool connected { get; set; }
        [DataMember]
        public bool overflow { get; set; }
        [DataMember]
        public object shape { get; set; }
        [DataMember]
        public TotalStands totalStands { get; set; }
        [DataMember]
        public MainStands mainStands { get; set; }
        [DataMember]
        public OverflowStands overflowStands { get; set; }
        [DataMember]
        public bool canPickUpABike { get; set; }
        [DataMember]
        public bool canDropOfABike { get; set; }

        public Boolean IsOpen()
        {
            return status.Equals("OPEN");
        }

        public void SetCanPickUpABike()
        {
            if (this.IsOpen() && (this.totalStands.availabilities.HasAnyBikeAvailable() || this.mainStands.availabilities.HasAnyBikeAvailable()))
            {
                this.canPickUpABike = true;
                return;
            }
            this.canPickUpABike = this.overflow && this.overflowStands != null && this.overflowStands.availabilities.HasAnyBikeAvailable();
        }

        public void SetCanDropOffABike()
        {
            if (this.IsOpen() && (this.totalStands.availabilities.HasAnAvailableStand() || this.mainStands.availabilities.HasAnAvailableStand()))
            {
                this.canDropOfABike = true;
                return;
            }
            this.canDropOfABike = this.overflow && this.overflowStands != null && this.overflowStands.availabilities.HasAnAvailableStand();
        }
    }

    [DataContract]
    public class Availabilities
    {
        [DataMember]
        public int bikes { get; set; }
        [DataMember]
        public int stands { get; set; }
        [DataMember]
        public int mechanicalBikes { get; set; }
        [DataMember]
        public int electricalBikes { get; set; }
        [DataMember]
        public int electricalInternalBatteryBikes { get; set; }
        [DataMember]
        public int electricalRemovableBatteryBikes { get; set; }

        public Boolean HasAnyBikeAvailable()
        {
            return bikes > 0;
        }

        public Boolean HasAnAvailableStand()
        {
            return stands > 0;
        }
    }

    [DataContract]
    public class MainStands
    {
        [DataMember]
        public Availabilities availabilities { get; set; }
        [DataMember]
        public int capacity { get; set; }
    }

    [DataContract]
    public class Position
    {
        [DataMember]
        public double latitude { get; set; }
        [DataMember]
        public double longitude { get; set; }
    }

    [DataContract]
    public class TotalStands
    {
        [DataMember]
        public Availabilities availabilities { get; set; }
        [DataMember]
        public int capacity { get; set; }
    }

    [DataContract]
    public class OverflowStands
    {
        [DataMember]
        public Availabilities availabilities { get; set; }
        [DataMember] 
        public int capacity { get; set; }
    }
}
