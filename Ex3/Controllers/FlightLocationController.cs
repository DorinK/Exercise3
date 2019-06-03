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
            if (!ValidateIPv4(param1))
            {
                string file = param1;
                int time = param2;
                model.ReadData(file);
                double infoLength = model.FileContent.Length;
                double timeout = (infoLength / 4) / (double)time;

                Session["timeLoad"] = time;
                Session["timeout"] = timeout;

                return View("loadFlightInfo");
            }

            string ip = param1;
            int port = param2;

            Session["ip"] = ip;
            Session["port"] = port;

            ConnectToServer.Instance(ip, port).Connect();
            //model.SimulatorConnection.Ip = ip;
            //model.SimulatorConnection.Port = port;
            //InfoModel.Instance.Start();
            return View("display");
        }

        private bool ValidateIPv4(string ipString)
        {
            if (ipString.Count(c => c == '.') != 3) return false;
            IPAddress address;
            return IPAddress.TryParse(ipString, out address);
        }

        [HttpGet]
        public ActionResult displayAndUpdate(string ip, int port, int time)
        {
            ConnectToServer.Instance(ip, port).Connect();

            Session["ip"] = ip;
            Session["port"] = port;
            Session["time"] = time;

            return View("displayAndUpdate");
        }

        [HttpGet]
        public ActionResult saveFlightInfo(string ip, int port, int time, int duration, string file)
        {
            InfoModel model = InfoModel.Instance;
            ConnectToServer.Instance(ip, port).Connect();
            model.PrepareFile(file);

            Session["ip"] = ip;
            Session["port"] = port;
            Session["timeSave"] = time;
            Session["duration"] = duration;
            Session["file_save"] = file;

            return View("saveFlightInfo");
        }

        [HttpPost]
        public string GetLocation()
        {
            var info = LocationPoint.Instance;
            info.Read(ConnectToServer.Instance(Session["ip"].ToString(), int.Parse(Session["port"].ToString())).Read());
            return ToXml(info);
        }

        [HttpPost]
        public string GetSaveSample()
        {
            var info = LocationPoint.Instance;
            info.Read(ConnectToServer.Instance(Session["ip"].ToString(),int.Parse(Session["port"].ToString())).Read());
            InfoModel.Instance.SaveData(Session["file_save"].ToString(), new double[] { info.Lon, info.Lat, info.Rudder, info.Throttle });
            return ToXml(info);
        }

        [HttpPost]
        public string ReadOnce()
        {
            var model = InfoModel.Instance;
            var info = LocationPoint.Instance;
            if (index < model.FileContent.Length)
            {
            info.Lon = double.Parse(model.FileContent[index++]);
            info.Lat = double.Parse(model.FileContent[index++]);
            info.Rudder = double.Parse(model.FileContent[index++]);
            info.Throttle = double.Parse(model.FileContent[index++]);
            }
            return ToXml(info);
        }

        private string ToXml(LocationPoint info)
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