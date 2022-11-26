using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    public class Station
    {
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }
        public DateTime lastUpdate { get; set; }
        public bool connected { get; set; }
        public bool overflow { get; set; }
        public object shape { get; set; }
        public TotalStands totalStands { get; set; }
        public MainStands mainStands { get; set; }
        public OverflowStands overflowStands { get; set; }

        public Boolean IsOpen()
        {
            return status.Equals("OPEN");
        }

        public Boolean CanPickUpABike()
        {
            if(this.IsOpen() && (this.totalStands.availabilities.HasAnyBikeAvailable() || this.mainStands.availabilities.HasAnyBikeAvailable()))
            {
                return true;
            }
            return this.overflow && this.overflowStands != null && this.overflowStands.availabilities.HasAnyBikeAvailable();
        }

        public Boolean CanDropOffABike()
        {
            if(this.IsOpen() && (this.totalStands.availabilities.HasAnAvailableStand() || this.mainStands.availabilities.HasAnAvailableStand()))
            {
                return true;
            }
            return this.overflow && this.overflowStands != null && this.overflowStands.availabilities.HasAnAvailableStand();
        }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
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

    public class MainStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class TotalStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class OverflowStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

}
