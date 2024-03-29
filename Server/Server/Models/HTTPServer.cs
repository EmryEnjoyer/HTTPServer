﻿using Server.Util;
using System.Net;
using System.Net.Sockets;
using System.Text;

// This class by Justice Roberts for IT3313
// Credit for archetype goes to MSDN developers
// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example

namespace Server.Models
{
    public class HTTPServer : IServer
    {
        public int Port { get; set; }
        public IPAddress Address { get; set; }
        public static ManualResetEvent connectDone = new ManualResetEvent(false);
        public static ManualResetEvent receiveDone = new ManualResetEvent(false);
        public static ManualResetEvent sendDone = new ManualResetEvent(false);
        public HTTPServer(int port, string address)
        {
            Port = port;
            Address = IPAddress.Parse(address);
        }
        /// <summary>
        /// Opens the socket created in the constructor
        /// </summary>
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
                    connectDone.Reset();
                    receiveDone.Reset();
                    Console.WriteLine("Listening...");
                    listener.BeginAccept(
                        new AsyncCallback(onAcceptCallback),
                        listener);
                    connectDone.WaitOne();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// Handles accepted connection asynchronously
        /// </summary>
        /// <param name="ar"></param>
        private void onAcceptCallback(IAsyncResult ar)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            connectDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            byte[] request = Receive(handler);
            receiveDone.WaitOne();
            string req = "";
            if (request[0] != 0)
            {
                req = Encoding.ASCII.GetString(request, 0, request.Length).Split(" ")[1];
            }
            byte[] data;
            if (req.Length > 0)
            {
                data = FileIO.BuildResponse(req.Substring(1));
            } else
            {
                data = FileIO.BuildResponse("missing");
            }
            Send(data, handler);
            sendDone.WaitOne();
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        /// <summary>
        /// Accepts request from socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>a byte array containing the received data</returns>
        public byte[] Receive(Socket socket)
        {
            byte [] data = new byte[2048];
            socket.BeginReceive(data, 0, 2048, SocketFlags.None, ReceiveCallback, socket);
            return data;
        }
        /// <summary>
        /// Handle received data asynchronously
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;    
                handler.EndReceive(ar);
                receiveDone.Set();
            } catch (ObjectDisposedException ex)
            {
                receiveDone.Set();
            }
        }
        /// <summary>
        /// Sends data through socket.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="socket"></param>
        public void Send(byte[] data, Socket socket)
        {
            socket.BeginSend(data,0,data.Length,SocketFlags.None, 
                new AsyncCallback(SendCallback), socket);
        }
        /// <summary>
        /// Handles the sending of data asynchronously
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
                sendDone.Set();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
