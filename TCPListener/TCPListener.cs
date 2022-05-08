using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    public class TCPListener
    {
        public static void Main()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 12000.
                Int32 port = 12000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // Buffer for reading data
                byte[] bytes = new byte[1024];
                String data = null;

                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                while (true)
                {
                    //Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    NetworkStream ns = client.GetStream();
                    //Console.WriteLine("Connected!");

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        //Translate data bytes to UTF8 string 
                        data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        //Console.WriteLine("Received: {0}", data);

                        //Process data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

                        //Send back response 
                        ns.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent: {0}", data);
                    }
                    //Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException {0}", e);
            }
            finally
            {
                //Stop listenging for a new clients 
                server.Stop();
            }
        }
    }
}
