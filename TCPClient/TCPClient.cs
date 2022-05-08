using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    public class TCPClient
    {
        public static void Connect(string server, string message)
        {
            try
            {
                Int32 port = 12000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into UTF8 and store it as a Byte array.
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", message);

                //Buffer to store the response bytes
                data = new byte[1024];

                // String to store the response UTF8 representation.
                string responseData = string.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                //Close everything
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }
    }
}
