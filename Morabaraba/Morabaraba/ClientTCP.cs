using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Morabaraba
{
    internal class ClientTCP
    {
        public static int playerNr = 1;
        public static void StartClient()
        {
            byte[] bytes = new byte[1024];
            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, int.Parse("8888"));

                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    sender.Connect(remoteEP);
                    playerNr++;
                    //Debug.WriteLine(playerNr);
                    Debug.WriteLine("Socket connected to {0}",sender.RemoteEndPoint.ToString());
                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(playerNr.ToString());

                    // Send the data through the socket.
                    GamePlay.whoStartsTheGame();
                    GamePlay.createPlayers();
                    int bytesSent = sender.Send(msg);
                    msg = Encoding.ASCII.GetBytes(GamePlay.myTurn1.ToString());
                    bytesSent = sender.Send(msg);

                    msg = Encoding.ASCII.GetBytes(GamePlay.myTurn2.ToString());
                    bytesSent = sender.Send(msg);
                    // Receive the response from the remote device.
                    //int bytesRec = sender.Receive(bytes);
                    //Debug.WriteLine("Echoed test = {0}",Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Debug.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Debug.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
