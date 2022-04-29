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
    public partial class Game : Form
    {
        public static Game game = new Game();
        private Board board;
        private PictureBox pictureBox;
        
        public Game()
        {
            InitializeComponent();

            board = new Board();
            pictureBox = new PictureBox();

            pictureBox.Width = pictureBox.Height = 1024;
            pictureBox.Image = Properties.Resources.game_board;
            pictureBox.Location = new System.Drawing.Point(0,0);
            pictureBox.MouseClick += PictureBox_MouseClick;

            for (int i = 0; i < board.panels.Length; i++)
            {
                board.panels[i].MouseClick += Game_MouseClick;
                this.Controls.Add(board.panels[i]);
            }
            this.Controls.Add(pictureBox);

        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Game_MouseClick(sender, e);
        }

        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            var panel = sender as Panel;
            if (null != panel)
            {
                board.panels[Int32.Parse(panel.Name) - 1].BackgroundImage = Properties.Resources.blackPiece;   
            }
        }

        private void buttonSurrender_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
