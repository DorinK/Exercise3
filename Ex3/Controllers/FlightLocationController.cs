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
        public ActionResult display(string ip, int port)
        {
            if (!ValidateIPv4(ip)) {
                string file = ip;
                int time = port;
                Session["timeLoad"] = time;
                Session["file_load"] = file;
                InfoModel.Instance.ReadData(file);
                Session["timeout"] = (InfoModel.Instance.FileContent.Length / 4) / time;
                return View("loadFlightInfo");
            }
           
            InfoModel.Instance.SimulatorConnection.Ip = ip;
            InfoModel.Instance.SimulatorConnection.Port = port;
            //InfoModel.Instance.Start();

            return View("display");
        }

        public static bool ValidateIPv4(string ipString)
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
            InfoModel.Instance.time = time;
            //InfoModel.Instance.Start(); 

            Session["time"] = time;

            return View();
        }

        [HttpGet]
        public ActionResult saveFlightInfo(string ip, int port, int time, int duration, string file)
        {
            InfoModel.Instance.SimulatorConnection.Ip = ip;
            InfoModel.Instance.SimulatorConnection.Port = port;
            InfoModel.Instance.time = time;
            //InfoModel.Instance.Start();

            Session["timeSave"] = time;
            Session["duration"] = duration;
            Session["file_save"] = file;

            return View(InfoModel.Instance);
        }

        [HttpGet]
        public ActionResult loadFlightInfo(string file, int time)
        {
            Session["timeLoad"] = time;
            Session["file_load"] = file;
            InfoModel.Instance.ReadData(file);
            return View();
        }

        [HttpPost]
        public string ReadOnce()
        {
            var info = InfoModel.Instance.location;
            info.Lon = int.Parse(InfoModel.Instance.FileContent[index++]);
            info.Lat = int.Parse(InfoModel.Instance.FileContent[index++]);
            info.Rudder = int.Parse(InfoModel.Instance.FileContent[index++]);
            info.Throttle = int.Parse(InfoModel.Instance.FileContent[index++]);
            return ToXml(info);

        }
        [HttpPost]
        public string GetLocation()
        {
            var info = InfoModel.Instance.location;
            info.read();
            return ToXml(info);
        }

        [HttpPost]
        public string GetSaveSample()
        {
            var info = InfoModel.Instance.location;
            info.ReadForSave();
            int[] data = { info.Lon, info.Lat, info.Rudder, info.Throttle };
            InfoModel.Instance.Save(Session["file_save"].ToString(), data);
            return ToXml(info);
        }

        /*[HttpPost]
        public string SaveInfo()
        {
            var info = InfoModel.Instance.location;
            int[] data = {info.Lon, info.Lat, info.Rudder, info.Throttle };

            //int[] data = { int.Parse(Session["lon"].ToString()), int.Parse(Session["lat"].ToString()), int.Parse(Session["rudder"].ToString()), int.Parse(Session["throttle"].ToString()) };
            InfoModel.Instance.Save(Session["file_save"].ToString(), data);
            return Session["file_save"].ToString();
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

        /*private string ToXmlSave(LocationPoint info)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("SaveSamples");

            info.ToXmlAllParams(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }*/

        // POST: First/Search
        //[HttpPost]
        /*public string Search(string name)
        {
            InfoModel.Instance.ReadData(name);

            return ToXml(InfoModel.Instance.Employee);
        }*/
    }
}