using Ex3.Models.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class LocationInfo
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }

        // Creating singleton for LocationPoint
        public static LocationInfo instace = null;
        public static LocationInfo Instance
        {
            get
            {
                if (instace == null)
                    instace = new LocationInfo();
                return instace;
            }
        }

        // Updating the values using the info came from the simulator.
        public void Read(Dictionary<string, double> dictionary)
        {
            Lon = dictionary["Lon"];
            Lat = dictionary["Lat"];
            Rudder = dictionary["Rudder"];
            Throttle = dictionary["Throttle"];
        }

        // Crating xml which contains the current location info.
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("LocationInfo");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Rudder", this.Rudder.ToString());
            writer.WriteElementString("Throttle", this.Throttle.ToString());
            writer.WriteEndElement();
        }
    }
}