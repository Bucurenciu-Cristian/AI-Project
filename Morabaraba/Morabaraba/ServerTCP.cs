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
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // Create a Socket that will use Tcp protocol

                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 2 requests at a time
                listener.Listen(2);
                Debug.WriteLine("Waiting for a connection...");
                DialogResult result = MessageBox.Show("Se asteapta conectarea jucatorilor");
                Socket handler = listener.Accept();

                // Incoming data from the client.
                string data = null;
                byte[] bytes = null;
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                ClientTCP.playerNr = int.Parse(data);
                if (ClientTCP.playerNr == 2)
                {
                    MessageBox.Show("jocul poate incepe!");
                }
                bytesRec = handler.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                int myturn1 = int.Parse(data);
                int myturn2 = myturn1 % 10;
                myturn1 = myturn1 / 10;
                GamePlay.setMyTurn1(myturn1);
                GamePlay.setMyTurn2(myturn2);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
