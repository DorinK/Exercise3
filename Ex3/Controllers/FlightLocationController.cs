using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class FlightLocationController : Controller
    {
        // GET: FlightLocation
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            InfoModel.Instance.SimulatorConnection.Ip = ip;
            InfoModel.Instance.SimulatorConnection.Port = port;
            //InfoModel.Instance.Start();

            return View();
        }

        [HttpGet]
        public ActionResult displayAndUpdate(string ip, int port, int time=1)
        {
            InfoModel.Instance.SimulatorConnection.Ip = ip;
            InfoModel.Instance.SimulatorConnection.Port = port;
            InfoModel.Instance.time = time;
            //InfoModel.Instance.Start(); 

            Session["time"] = time;

            return View();
        }

        [HttpGet]
        public ActionResult saveInfo(string ip, int port, int time,int duration,string file)
        {
            InfoModel.Instance.SimulatorConnection.Ip = ip;
            InfoModel.Instance.SimulatorConnection.Port = port;
            InfoModel.Instance.time = time;
            //InfoModel.Instance.Start();

            Session["time"] = time;

            return View();
        }

        [HttpGet]
        public ActionResult displayFromFile(string file, int time)
        {
            return View();
        }

        /*[HttpPost]
        public string GetFlightLocation()
        {
            var info = InfoModel.Instance.SimulatorConnection;
            //info.read();
            return ToXml(info);
        }*/

        [HttpPost]
        public string GetLocation()
        {
            var info = InfoModel.Instance.location;
            info.read();
            return ToXml(info);
        }

        private string ToXml(LocationPoint info)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("SimulatorConnections");

            info.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        // POST: First/Search
        //[HttpPost]
        /*public string Search(string name)
        {
            InfoModel.Instance.ReadData(name);

            return ToXml(InfoModel.Instance.Employee);
        }*/
    }
}