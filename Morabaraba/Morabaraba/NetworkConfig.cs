using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    public partial class NetworkConfig : Form
    {
        public static NetworkConfig net = new NetworkConfig();
        public NetworkConfig()
        {
            InitializeComponent();
        }
        public void Init()
        {
            NetworkConfig.setIP("127.0.0.1");
            NetworkConfig.setPort("8888");
        }
        public static string getIP()
        {
            return net.textBoxIP.Text;
        }
        public static string getPort()
        {
            return net.textBoxPort.Text;
        }
        public static void setIP(string IP)
        {
            net.textBoxIP.Text = IP;
        }
        public static void setPort(string port)
        { 
            net.textBoxPort.Text = port;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            ServerTCP.StartServer();
            Game.game.Show();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
