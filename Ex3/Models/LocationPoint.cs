using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class LocationPoint
    {
        private static Random rnd = new Random();

        public int Lon { get; private set; }
        public int Lat { get; private set; }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("LocationPoint");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteEndElement();
        }

        public void read()
        {
            //read from FG server.
            this.Lon = rnd.Next(360);
            this.Lat = rnd.Next(180);
        }
    }
}