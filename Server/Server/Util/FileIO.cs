using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Util
{
    public class FileIO
    {
        public static byte[] BuildResponse(string path) 
        {
            var response = 200;                                     // Keep track of response through 
                                                                    // modification
            byte[] buffer;
            try
            {
                var f = File.OpenRead(path);
                var fi = new FileInfo(path);
                buffer = new Byte[fi.Length];
                f.Read(buffer, 0, buffer.Length);
                f.Flush();
                f.Close();
            } catch (FileNotFoundException ex)
            {
                buffer = new byte[0];
                response = 404;
            }
            byte[] headers = BuildHeaders(response);
            byte[] msg = new byte[buffer.Length + headers.Length]; // Message is headers + body

            for(int i = 0; i < headers.Length; i++)                // Put headers on message
            {
                msg[i] = headers[i];
            }
            for (int i = headers.Length-1; i < buffer.Length+headers.Length-1; i++) // Put body on message
            {
                msg[i] = buffer[i-headers.Length+1];
            }

            return msg;
        }
        public static byte[] BuildHeaders(int responseCode)
        {
            var msg = "";
            if(responseCode == 404)
            {
                msg = "HTTP/1.1 404 NOT FOUND";
            }
            else if(responseCode == 200)
            {
                msg = "HTTP/1.1 200 OK\r\n";
                msg += "Access-Control-Allow-Origin: *\r\n";
                msg += "Cache-Control: no-cache" + "\r\n";
                msg += "Content-Type: text/html; charset=utf-8\r\n";
            }
            msg += "\r\n\r\n";
            return System.Text.Encoding.UTF8.GetBytes(msg);
        }
    }
}
