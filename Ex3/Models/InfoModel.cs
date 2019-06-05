using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class InfoModel
    {
        // Creating singleton for InfoModel
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

        // Will store the content of the file we have been asked to load.
        public string[] FileContent { get; private set; }

        // The Path of the Secnario
        private const string SCENARIO_FILE = "~/App_Data/{0}.txt";           

        // If the file exists we clear it's content.
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

        // Save the samples of the flight location to the requested file.
        public void SaveData(string fileName, double[] sample)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            using (writer = new System.IO.StreamWriter(path, true))
            {
                // Saving the params describe the flight location of one sample.
                foreach (double param in sample)
                    writer.WriteLine(param);
            }
        }

        // Reading the flight locations from the requested file and saving it in the FileContent array.
        public void ReadData(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            FileContent = System.IO.File.ReadAllLines(path);
        }
    }
}