using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    public partial class Game : Form
    {
        private Board board;
        private PictureBox pictureBox;
        private GameState gameState;
        public enum GameState
        {
            AgainstPC,
            AgainstPlayer
        }   
        
        public Game(GameState theState, int playerIndex)
        {
            InitializeComponent();
            board = new Board();
            pictureBox = new PictureBox();
            gameState = theState;

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
            if (playerIndex==1)
            {
                if(gameState == GameState.AgainstPC)
                {
                    GamePlay.whoStartsTheGame();
                    GamePlay.createPlayers();
                    SetPlayerName(GamePlay.player1.getName(),"PC");
                }
                else if(gameState == GameState.AgainstPlayer)
                {
                    GamePlay.createPlayers();
                    SetPlayerName(GamePlay.player1.getName(), "");
                }
            }
            else
            {
                if (playerIndex==2 && gameState==GameState.AgainstPlayer)
                {
                    SetPlayerName(GamePlay.player2.getName(), "");
                }
            }
            SetPlayerTurn(GamePlay.myTurn1, GamePlay.myTurn2);
            setTextFromTextBox("Playerul " + GamePlay.player1.getName() + " are randul " + GamePlay.player1.myTurn + " si culoarea " + (GamePlay.player1.myColor==true ? "alb":"negru") + "\r\n");
            setTextFromTextBox("Playerul " + GamePlay.player2.getName() + " are randul " + GamePlay.player2.myTurn + " si culoarea " + (GamePlay.player2.myColor==true ? "alb":"negru") + "\r\n");
            //GamePlay.playerPlacing();
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            string currentPlayerName = labelName1.Text;
            Player activePlayer = GamePlay.getActivePlayer();
            string activePlayerName = activePlayer.getName();
            if (currentPlayerName.CompareTo(activePlayerName)==0)
            {
                Game_MouseClick(sender, e);
            }
        }

        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            string currentPlayerName = labelName1.Text;
            Player activePlayer = GamePlay.getActivePlayer();
            string activePlayerName = activePlayer.getName();
            Debug.WriteLine(GamePlay.getActivePlayer().getName()+" player care poate muta");
            Debug.WriteLine(currentPlayerName+ " player care vrea sa mute");
            if (currentPlayerName.CompareTo(activePlayerName) == 0)
            {
                var panel = sender as Panel;
                if (null != panel)
                {
                    //Debug.WriteLine(GamePlay.getActivePlayer().getName());
                    board.panels[Int32.Parse(panel.Name) - 1].BackgroundImage = GamePlay.getActivePlayer().getMyColor();
                    Player inactivePlayer = GamePlay.getInactivePlayer();
                    if(activePlayer.myHandCells.Count()>0)
                    {
                        GamePlay.playerTurn(activePlayer, inactivePlayer, activePlayer.myHandCells.Count);
                    }
                    else
                    {
                        MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                    }
                }
            }     
        }

        private void buttonSurrender_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string getTextFromTextBox()
        {
            return this.textBoxGameLog.Text;
        }
        public void setTextFromTextBox(string newText)
        {
            this.textBoxGameLog.Text += newText;
        }

        public void SetPlayerName(string name1, string name2)
        {
            this.labelName1.Text = name1;
            this.labelName2.Text = name2;
        }
        public void SetPlayerTurn(int myTurn1, int myTurn2)
        {
            this.label1.Text = myTurn1.ToString();
            this.label2.Text = myTurn2.ToString();
        }
    }
}
