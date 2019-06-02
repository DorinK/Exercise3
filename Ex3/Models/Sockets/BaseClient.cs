using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex3.Models.Sockets
{
    abstract class BaseClient : ITelnetClient
    {
        public abstract string Ip { get; }  // Return ip adress.
        public abstract int Port { get; }   // Return port number.

        public delegate void ClientEvent(); // Client events delegate.
        public event ClientEvent NotifyConnected;   // Client event for connection.
        public event ClientEvent NotifyDisconnected;    // Client event for disconnected.

        public abstract void Connect(); // Connect to client.
        public abstract void ReConnect(string ip, int port);    // Reconnect to ip and port given.
        public abstract void Write(string command); // Write to client msg.
        public abstract Dictionary<string,double> Read();
        public abstract void Disconnect();  // Disconnect from client.

        // Notify that client connected.
        public void NotifyClientConnectedEvent()
        {
            ClientEvent handler = NotifyConnected;
            NotifyConnected?.Invoke();
        }

        // Notify that client disconnected.
        public void NotifyClientDisconnectedEvent()
        {
            ClientEvent handler = NotifyDisconnected;
            NotifyDisconnected?.Invoke();
        }
    }
}
