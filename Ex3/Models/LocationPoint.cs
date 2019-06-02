using Ex3.Models.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class LocationPoint
    {
        //private static Random rnd = new Random();

        public int Lon { get; set; }
        public int Lat { get;  set; }
        public int Rudder { get;  set; }
        public int Throttle { get;  set; }

        /*public double Lon { get; set; }
        public double Lat { get; set; }
        public double Rudder { get; set; }
        public double Throttle { get; set; }*/
        
        /*public void Read(BaseClient simulator)
        {
            Dictionary<string,double> dictionary= simulator.Read();
            Lon = dictionary["Lon"];
            Lat = dictionary["Lat"];
            Rudder = dictionary["Rudder"];
            Throttle = dictionary["Throttle"];
        }*/

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("LocationPoint");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Rudder", this.Rudder.ToString());
            writer.WriteElementString("Throttle", this.Throttle.ToString());
            writer.WriteEndElement();
        }
        /*public void read()
        {
            //read from FG server.
            this.Lon = rnd.Next(360);
            this.Lat = rnd.Next(180);
        }

        public void ReadForSave()
        {
            //read from FG server.
            Lon = rnd.Next(360);
            Lat = rnd.Next(180);
            Rudder = rnd.Next(124);
            Throttle = rnd.Next(124);
        }

        */

    }
}