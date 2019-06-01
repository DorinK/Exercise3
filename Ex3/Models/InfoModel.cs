using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class InfoModel
    {
        private static InfoModel s_instace = null;
        public static InfoModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new InfoModel();
                }
                return s_instace;
            }
        }

        //public Employee Employee { get; private set; }
        public string ip { get; set; }
        public string port { get; set; }
        public int time { get; set; }
        public LocationPoint location { get; }
        public SimulatorConnection SimulatorConnection { get; private set; }
        public string[] FileContent { get; private set; }

        public InfoModel()
        {
            //Employee = new Employee();
            SimulatorConnection = new SimulatorConnection();
            location = new LocationPoint();
            //SimulatorConnection = FlightGearModel.Instance();

        }

        /*public void Start()
        {
            //NetworkConnection.Connect();
            SimulatorConnection.read();
        }*/

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public void Save(string fileName, int[] samples)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            if (!File.Exists(path))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    for (int i = 0; i < samples.Length; i++)
                    {
                        file.WriteLine(samples[i]);
                    }
                }
            }
            else
            {
                /*FileInfo fi = new FileInfo(path);
                using (TextWriter toFile = new StreamWriter(fi.Open(FileMode.Truncate)))
                {
                    for (int i = 0; i < samples.GetLength(0); i++)
                    {
                        for (int j = 0; j < samples.GetLength(1); j++)
                            toFile.WriteLine(samples[i, j]);
                    }
                    //txtWriter.Write("Write your line or content here");
                }*/
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    for (int i = 0; i < samples.Length; i++)
                    {
                        file.WriteLine(samples[i]);
                    }
                }
            }
        }

        /*public void SaveData(string fileName, int[,] samples)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            if (!File.Exists(path))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    for (int i = 0; i < samples.GetLength(0); i++)
                    {
                        for (int j = 0; j < samples.GetLength(1); j++)
                            file.WriteLine(samples[i, j]);
                    }
                }
            }
            else
            {
                FileInfo fi = new FileInfo(path);
                using (TextWriter toFile = new StreamWriter(fi.Open(FileMode.Truncate)))
                {
                    for (int i = 0; i < samples.GetLength(0); i++)
                    {
                        for (int j = 0; j < samples.GetLength(1); j++)
                            toFile.WriteLine(samples[i, j]);
                    }
                    //txtWriter.Write("Write your line or content here");
                }
            }
        }*/

        public void ReadData(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            FileContent = System.IO.File.ReadAllLines(path);
        }


    }
}