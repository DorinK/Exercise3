using Ex3.Models;
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
                InfoModel.Instance.ReadData(file);
                double infoLength = model.FileContent.Length;
                double timeout = (infoLength / 4) / (double)time;

                Session["timeLoad"] = time;
                Session["timeout"] = timeout;

                return View("loadFlightInfo");
            }

            string ip = param1;
            int port = param2;
            model.SimulatorConnection.Ip = ip;
            model.SimulatorConnection.Port = port;
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
            InfoModel.Instance.SimulatorConnection.Ip = ip;
            InfoModel.Instance.SimulatorConnection.Port = port;
            //InfoModel.Instance.Start(); 

            Session["time"] = time;

            return View("displayAndUpdate");
        }

        [HttpGet]
        public ActionResult saveFlightInfo(string ip, int port, int time, int duration, string file)
        {
            InfoModel model = InfoModel.Instance;
            model.SimulatorConnection.Ip = ip;
            model.SimulatorConnection.Port = port;
            //InfoModel.Instance.Start();
            model.PrepareFile(file);

            Session["timeSave"] = time;
            Session["duration"] = duration;
            Session["file_save"] = file;

            return View();
        }

        /*[HttpGet]
        public ActionResult loadFlightInfo(string file, int time)
        {
            Session["timeLoad"] = time;
            Session["file_load"] = file;
            InfoModel.Instance.ReadData(file);
            return View();
        }*/

        [HttpPost]
        public string GetLocation()
        {
            var info = InfoModel.Instance.Location;
            //info.Read();
            return ToXml(info);
        }

        [HttpPost]
        public string GetSaveSample()
        {
            var info = InfoModel.Instance.Location;
            //info.Read();
            InfoModel.Instance.SaveData(Session["file_save"].ToString(), new int[] { info.Lon, info.Lat, info.Rudder, info.Throttle });
            return ToXml(info);
        }

        [HttpPost]
        public string ReadOnce()
        {
            var model = InfoModel.Instance;
            var info = model.Location;
            if (index < model.FileContent.Length)
            {
            info.Lon = int.Parse(model.FileContent[index++]);
            info.Lat = int.Parse(model.FileContent[index++]);
            info.Rudder = int.Parse(model.FileContent[index++]);
            info.Throttle = int.Parse(model.FileContent[index++]);
            }
            return ToXml(info);
        }

        /*public ActionResult check()
        {
            if (index >= InfoModel.instace.FileContent.Length)
                return Content("true");
            return Content("false");
        }*/
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