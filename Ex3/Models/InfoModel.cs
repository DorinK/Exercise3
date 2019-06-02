using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class InfoModel
    {
        public static InfoModel instace = null;
        public static InfoModel Instance
        {
            get
            {
                if (instace == null)
                    instace = new InfoModel();
                return instace;
            }
        }

        private System.IO.StreamWriter writer;
        public SimulatorConnection SimulatorConnection { get; private set; }
        public LocationPoint Location { get; }
        public string[] FileContent { get; private set; }
        public int Index { get;  set; }

        public InfoModel()
        {
            Index = 0;
            SimulatorConnection = new SimulatorConnection();
            Location = new LocationPoint();
        }

        /*public void Start()
        {
            //NetworkConnection.Connect();
            SimulatorConnection.read();
        }*/

        private const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public void PrepareFile(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            if (File.Exists(path))
            {
                FileStream fileStream = File.Open(path, FileMode.Open);
                fileStream.SetLength(0);
                fileStream.Close(); // This flushes the content, too.
            }
        }

        public void SaveData(string fileName, int[] samples)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            using (writer = new System.IO.StreamWriter(path, true))
            {
                foreach (int param in samples)
                    writer.WriteLine(param);
            }
        }

        public void ReadData(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            FileContent = System.IO.File.ReadAllLines(path);
        }
    }
}