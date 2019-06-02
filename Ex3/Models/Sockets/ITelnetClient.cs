using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex3.Models
{
    interface ITelnetClient // Interface connect as client
    {
        string Ip { get; } // Return ip adress.
        int Port { get; } // Return port number.
        void Connect(); // Connect to client.
        void ReConnect(string ip, int port); // Reconnect to ip and port given.
        void Write(string command); // Write to client msg.
        Dictionary<string, double> Read();
        void Disconnect(); // Disconnect from client.
    }
}
