﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex3.Models.Sockets
{
    class SimulatorConnection : BaseClient
    {
        IPEndPoint ep;
        TcpClient client;

        // Get ip from IP object.
        public override string Ip { get => ep.Address.ToString(); }

        // Get port from port object.
        public override int Port { get => ep.Port; }

        private string Lon_Path { get; } = "/position/longitude-deg";
        private string Lat_Path { get; } = "/position/latitude-deg";
        private string Rudder_Path { get; } = "/controls/flight/rudder";
        private string Throttle_Path { get; } = "/controls/engines/current-engine/throttle";

        // Construct client ip end point.
        private SimulatorConnection(string ip, int port) => ep = new IPEndPoint(IPAddress.Parse(ip), port);

        private static BaseClient instance = null;
        public static BaseClient Instance(string ip, int port)
        {

            if (instance == null)
                instance = new SimulatorConnection(ip, port);
            return instance;
        }

        // Connect to client, create task and loop.
        public override void Connect()
        {
            new Task(() =>
            {
                // Create tcp client.
                client = new TcpClient();
                try
                {
                    client.Connect(ep);     // Connect to client.
                    this.NotifyClientConnectedEvent();      // Notify connection.

                    // disconnect set client to null(free socket).
                    while (client != null)
                    {
                        // read every 10HZ seconds.
                        Thread.Sleep(100);
                    }
                }

                catch (Exception)
                {
                    Disconnect();   // Disconnect notify and disconnect operation.
                }
            }).Start();
        }

        public override void ReConnect(string ip, int port)
        {
            // Disconnect , create new IP end point, create connection again.
            Disconnect();
            ep = new IPEndPoint(IPAddress.Parse(ip), port);
            Connect();
        }

        // Disconnect from the client.
        public override void Disconnect()
        {
            // Notify disconnection.
            NotifyClientDisconnectedEvent();

            // Close if client object exist.
            if (this.client != null)
            {
                client.Client.Close();      // Close socket and then client.
                client.Close();
                this.client = null;     // Insurance that client closed.

            }

        }

        // Parse string from the simulator
        private string ParseValue(string toBeParsed)
        {
            string[] result = toBeParsed.Split('=');
            result = result[1].Split('\'');
            result = result[1].Split('\'');

            return result[0];
        }

        // Write to client msg.
        public override void Write(string command)
        {
            try
            {
                // Send data to server, first encode it.
                byte[] myWriteBuffer = Encoding.ASCII.GetBytes(command + "\r\n");
                client.GetStream().Write(myWriteBuffer, 0, myWriteBuffer.Length);
            }
            catch (Exception)
            {
                // If exepction happened, problem with client.
                NotifyClientDisconnectedEvent();
            }
        }

        // Read one param each time
        private double ReadOnce(string path)
        {
            try
            {
                byte[] myWriteBuffer = Encoding.ASCII.GetBytes("get " + path + "\r\n");
                client.GetStream().Write(myWriteBuffer, 0, myWriteBuffer.Length);
                byte[] bytes = new byte[client.ReceiveBufferSize];
                client.GetStream().Read(bytes, 0, client.ReceiveBufferSize);
                string returnedData = Encoding.ASCII.GetString(bytes);

                // parse the string
                string parsed = ParseValue(returnedData);
                return double.Parse(parsed);
            }
            catch (Exception)
            {
                NotifyClientDisconnectedEvent();
            }
            return double.NaN;
        }

        // Read all params one by one.
        public override Dictionary<string, double> Read()
        {
            var values = new Dictionary<string, double>()
            {
                ["Lon"] = ReadOnce(Lon_Path),
                ["Lat"] = ReadOnce(Lat_Path),
                ["Rudder"] = ReadOnce(Rudder_Path),
                ["Throttle"] = ReadOnce(Throttle_Path)
            };
            return values;
        }
    }
}