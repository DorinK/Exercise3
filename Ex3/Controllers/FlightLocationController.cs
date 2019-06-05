using Ex3.Models;
using Ex3.Models.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class FlightLocationController : Controller
    {
        private static int index = 0;

        // GET: FlightLocation
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        
        public ActionResult display(string param1, int param2)
        {
            var model = InfoModel.Instance;
            // display flight info from file
            if (!ValidateIPv4(param1))
            {
                string file = param1;
                int time = param2;
                model.ReadData(file);
                double infoLength = model.FileContent.Length;
                //defining timeout
                double timeout = (infoLength / 4) * (double)time;

                Session["timeLoad"] = time;
                Session["timeout"] = timeout;

                return View("loadFlightInfo");
            }

            // desplaying only current location
            string ip = param1;
            int port = param2;

            Session["ip"] = ip;
            Session["port"] = port;

            // connect to simulator.
            SimulatorConnection.Instance(ip, port).Connect();
            return View("display");
        }

        // Chacks if the string is valid ip.
        private bool ValidateIPv4(string ipString)
        {
            if (ipString.Count(c => c == '.') != 3) return false;
            IPAddress address;
            return IPAddress.TryParse(ipString, out address);
        }

        [HttpGet]
        // Sampling the flight info with no time limit.
        public ActionResult displayAndUpdate(string ip, int port, int time)
        {
            SimulatorConnection.Instance(ip, port).Connect();

            Session["ip"] = ip;
            Session["port"] = port;
            Session["time"] = time;

            return View("displayAndUpdate");
        }

        [HttpGet]
        // Saving the flight info samples to the requested file and showing the path.
        public ActionResult saveFlightInfo(string ip, int port, int time, int duration, string file)
        {
            InfoModel model = InfoModel.Instance;
            SimulatorConnection.Instance(ip, port).Connect();
            model.PrepareFile(file);

            Session["ip"] = ip;
            Session["port"] = port;
            Session["timeSave"] = time;
            Session["duration"] = duration;
            Session["file_save"] = file;

            return View("saveFlightInfo");
        }

        [HttpPost]
        // Returns an xml with the current location info.
        public string GetLocation()
        {
            var info = LocationInfo.Instance;
            info.Read(SimulatorConnection.Instance(Session["ip"].ToString(), int.Parse(Session["port"].ToString())).Read());
            return ToXml(info);
        }

        [HttpPost]
        // Sampling the flight info from the simulator.
        public string GetSaveSample()
        {
            var info = LocationInfo.Instance;
            info.Read(SimulatorConnection.Instance(Session["ip"].ToString(),int.Parse(Session["port"].ToString())).Read());
            InfoModel.Instance.SaveData(Session["file_save"].ToString(), new double[] { info.Lon, info.Lat, info.Rudder, info.Throttle });
            return ToXml(info);
        }

        [HttpPost]
        // Reading one sample 
        public string ReadOnce()
        {
            var model = InfoModel.Instance;
            var info = LocationInfo.Instance;

            // As long we can still read.
            if (index < model.FileContent.Length)
            {
            info.Lon = double.Parse(model.FileContent[index++]);
            info.Lat = double.Parse(model.FileContent[index++]);
            info.Rudder = double.Parse(model.FileContent[index++]);
            info.Throttle = double.Parse(model.FileContent[index++]);
            }
            return ToXml(info);
        }

        // Creating an xml.
        private string ToXml(LocationInfo info)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("FlightLocations");

            info.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }
    }
}