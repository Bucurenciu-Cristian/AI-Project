using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPlayer_Click(object sender, EventArgs e)
        {
            this.Hide();
            NetworkConfig.net.Show();
            NetworkConfig.net.Init();
        }

        private void buttonPC_Click(object sender, EventArgs e)
        {
            Game game = new Game(Game.GameState.AgainstPC,1);
            game.InitalizeGame();
            game.Show();
        }
    }
}
