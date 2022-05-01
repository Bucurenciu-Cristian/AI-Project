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
        private const int MaxInstanceCount = 2;
        private static readonly Semaphore Semaphore = new Semaphore(MaxInstanceCount, MaxInstanceCount, "CanRunTwice");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Semaphore.WaitOne(1000))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    int count = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length;
                    Debug.WriteLine(count + "-----------------------");
                    if (count == 1)
                    {
                        Application.Run(new Menu());
                    }
                    else if (count == 2)
                    {
                        //ClientTCP.StartClient();
                        Game game2 = new Game(Game.GameState.AgainstPlayer, 2);
                        game2.InitalizeGame();
                        MessageBox.Show("jocul poate incepe!");
                        Application.Run(game2);               
                    }
                }
                finally
                {
                    Semaphore.Release();
                }
            }
            else
            {
                Debug.WriteLine("I cannot run, too many instances are already running");
            }
        }
    }
}
