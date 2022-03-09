using Server.Util;
using System.Net;
using System.Net.Sockets;

// This class by Justice Roberts for IT3313
// Credit for archetype goes to MSDN developers
// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example

namespace Server.Models
{
    public class HTTPServer : IServer
    {
        public int Port { get; set; }
        public IPAddress Address { get; set; }
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public HTTPServer(int port, string address)
        {
            Port = port;
            Address = IPAddress.Parse(address);
        }
        public async Task<Socket> BroadcastAsync()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            var listener = new Socket(Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(Address, Port);
            try
            {
                listener.Bind(ep);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    Console.WriteLine("Listening...");
                    listener.BeginAccept(
                        new AsyncCallback(onAccept),
                        listener);
                    allDone.WaitOne();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void onAccept(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            byte[] data = FileIO.BuildResponse("Test");

            Send(data, handler);
        }
        public async Task<Socket> ReceiveAsync()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] data, Socket socket)
        {
            socket.BeginSend(data,0,data.Length,SocketFlags.None, 
                new AsyncCallback(SendCallback), socket);
        }
        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
