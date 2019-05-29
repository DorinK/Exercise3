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
        public LocationPoint location{get;}
        public SimulatorConnection SimulatorConnection { get; private set; }

        public InfoModel()
        {
            //Employee = new Employee();
            SimulatorConnection=new SimulatorConnection();
            location = new LocationPoint();
            //SimulatorConnection = FlightGearModel.Instance();

        }

        /*public void Start()
        {
            //NetworkConnection.Connect();
            SimulatorConnection.read();
        }*/

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";           // The Path of the Secnario

        public void ReadData(string name)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, name));
            if (!File.Exists(path))
            {
                //Employee.FirstName = name;
                //Employee.LastName = name;
                //Employee.Salary = 500;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    //file.WriteLine(Employee.FirstName);
                    //file.WriteLine(Employee.LastName);
                    //file.WriteLine(Employee.Salary);
                }
            }
            else
            {
                string[] lines = System.IO.File.ReadAllLines(path);        // reading all the lines of the file
                //Employee.FirstName = lines[0];
                //Employee.LastName = lines[1];
                //Employee.Salary = int.Parse(lines[2]);
            }
        }
    }
}