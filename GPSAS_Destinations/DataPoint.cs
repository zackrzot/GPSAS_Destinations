using System;

namespace GPSAS_Destinations
{
    public class DataPoint
    {
        // Variables
        public int AID { get; set; }
        public int IID { get; set; }
        public String ID { get; set; }
        public Double LAT { get; set; }
        public Double LON { get; set; }
        public String SETTING { get; set; }
        public DateTime DATETIME { get; set; }

        // Constructor
        public DataPoint(String _id, Double _lat, Double _lon, String _setting, DateTime _dateTime)
        {
            this.AID = ClusterComputer.UNMARKED;
            this.IID = ClusterComputer.UNMARKED;
            this.ID = _id;
            this.LAT = _lat;
            this.LON = _lon;
            this.SETTING = _setting;
            this.DATETIME = _dateTime;
        }

    }
}
