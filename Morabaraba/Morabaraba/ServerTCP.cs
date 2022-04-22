using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Morabaraba
{
    internal class ServerTCP
    {
        public static void StartServer()
        {
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            IPAddress ipAddress = IPAddress.Parse(NetworkConfig.getIP());
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, int.Parse(NetworkConfig.getPort()));

            try
            {

                // Create a Socket that will use Tcp protocol
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                listener.Listen(2);
                Debug.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();
                DialogResult result = MessageBox.Show("Se asteapta conectarea jucatorilor");
                if (result == DialogResult.OK)
                {
                    /*while(ClientTCP.playerNr!=2)
                    {
                        MessageBox.Show("playeri conectati "+ ClientTCP.playerNr);
                    }*/
                    MessageBox.Show("jocul poate incepe!");
                }

                // Incoming data from the client.
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }
                Console.WriteLine("Text received : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            Debug.WriteLine("\n Press any key to continue...");
            //Debug.ReadKey();
        }
    }
}
