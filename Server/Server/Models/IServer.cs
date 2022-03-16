using System.Net;
using System.Net.Sockets;

namespace Server.Models
{
    public interface IServer
    {
        public int Port { get; set; }
        public IPAddress Address { get; set; }
        public void Open();
        public byte[] Receive(Socket socket);
        public void Send(byte[] data, Socket socket);
    }
}
