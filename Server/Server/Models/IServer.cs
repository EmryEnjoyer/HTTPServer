using System.Net;
using System.Net.Sockets;

namespace Server.Models
{
    public interface IServer
    {
        public int Port { get; set; }
        public IPAddress Address { get; set; }
        public void Open();
        public void onAccept(IAsyncResult ar);
        public void Close();
        public byte[] Receive(Socket socket);
        public void Send(byte[] data, Socket socket);
        public Task<Socket> BroadcastAsync();
    }
}
