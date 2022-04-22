using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClientTCP.StartClient();
            //Debug.WriteLine(NetworkConfig.getIP()+ "nu");
            if (ClientTCP.playerNr == 1)
            {
                Application.Run(new Menu());
            }
            else if(ClientTCP.playerNr == 2)
            {
                Application.Run(new Game());
            }

            //Application.Run(new NetworkConfig());
            //Application.Run(new Game());
        }
    }
}
