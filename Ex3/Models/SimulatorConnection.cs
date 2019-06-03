using Ex3.Models.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class SimulatorConnection
    {
        //public string Ip { get; set; }
        //public int Port { get; set; }
        /*public BaseClient Server { get; set; } = null;

        private SimulatorConnection(string ip, int port)
        {
            Server = new ConnectToServer(ip, port);
        }*/

        /*private static SimulatorConnection s_instace = null;

        public static SimulatorConnection Instance(string ip, int port)
        {
            if (s_instace == null)
                s_instace = new SimulatorConnection(ip, port);
            return s_instace;
        }*/

        /*public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("SimulatorConnection");
            writer.WriteElementString("Ip", this.Ip);
            writer.WriteElementString("Port", this.Port.ToString());
            writer.WriteEndElement();
        }*/
    }
}