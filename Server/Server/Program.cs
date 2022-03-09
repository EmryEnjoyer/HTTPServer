using Server.Models;


IServer server = new HTTPServer(12340, "0.0.0.0");
server.Open();

// var port = 12349;
// 
// IPAddress address = IPAddress.Parse("0.0.0.0");
// IPEndPoint endPoint = new IPEndPoint(address, port);
// Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
// socket.Bind(endPoint);
// var msg = Console.ReadLine();
// while(msg == null)
// {
//     msg = Console.ReadLine();
// }
// var fileName = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];


// socket.Listen(5);
// while (true)
// {
//     Console.WriteLine("Ready to serve...");
//     var f = File.Open(fileName, FileMode.Open);
//     var fi = new FileInfo(fileName);
//     var clientSocket = socket.Accept();
//     try
//     {
//         Byte[] buffer = new Byte[fi.Length];
//         f.Read(buffer, 0, buffer.Length);
//         f.Close();
//         var headers = "HTTP/1.1 200 OK" + "\r\n";
//         headers += "Access-Control-Allow-Origin: *";
//         headers += "Cache-Control: no-cache" + "\r\n";
//         headers += "Content-Type: text/html; charset=utf-8\r\n";
//         headers += "\r\n\r\n";
//         Byte[] httpHeaders = System.Text.Encoding.UTF8.GetBytes(headers);
//         await clientSocket.SendAsync(new ArraySegment<Byte>(httpHeaders), SocketFlags.None);
//         await clientSocket.SendAsync(new ArraySegment<Byte>(buffer), SocketFlags.None);
//     }
//     catch (Exception ex)
//     {
//         if (ex is SocketException)
//             Console.WriteLine(ex.ToString());
//     }
// }
