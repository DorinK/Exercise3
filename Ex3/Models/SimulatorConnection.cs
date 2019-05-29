using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class SimulatorConnection
    {

        public string Ip { get; set; }
        public int Port { get; set; }
        //public int Lat { get; set; }
        //public int Lon { get; set; }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("SimulatorConnection");
            writer.WriteElementString("Ip", this.Ip);
            writer.WriteElementString("Port", this.Port.ToString());
            //writer.WriteElementString("Lat", this.Lat.ToString());
            //writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteEndElement();
        }

        
    }
}