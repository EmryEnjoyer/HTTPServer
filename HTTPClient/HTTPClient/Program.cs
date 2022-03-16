
using System.Net;
using System.Net.Sockets;

if (args.Length < 3)
{
    Console.WriteLine("usage: HTTPClient <host> <port> <filename>");
    return;
}
string addr = args[0];
int port = int.Parse(args[1]);
string fname = args[2];
IPAddress address = IPAddress.Parse(addr);
EndPoint endPoint = new IPEndPoint(address, port);
Socket sckt = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
await sckt.ConnectAsync(endPoint);
Console.WriteLine("Connected to {0}", sckt.RemoteEndPoint.ToString());
byte[] msg = BuildMessage(fname, addr);
ArraySegment<byte> buffer = new ArraySegment<byte>(msg);
await sckt.SendAsync(buffer, SocketFlags.None);
byte[] recvBuff = new byte[2048];
await sckt.ReceiveAsync(recvBuff, SocketFlags.None);
Console.WriteLine(System.Text.Encoding.UTF8.GetString(recvBuff));
static byte[] BuildMessage(string fname, string addr)
{
    string msg = "GET /" + fname + " HTTP 1.1\r\n";
    msg += "Host: " + addr + "\r\n";
    msg += "Accept: text/html\r\n";
    Console.WriteLine(msg);
    return System.Text.Encoding.UTF8.GetBytes(msg);
}