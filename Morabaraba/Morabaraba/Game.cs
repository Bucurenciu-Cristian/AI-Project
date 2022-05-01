using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        private Socket socket;//serverSocket vs clientSocket
        private int playerIndex;
        private BackgroundWorker messageReceiver = new BackgroundWorker();
        private static string data="";

        public enum GameState
        {
            AgainstPC,
            AgainstPlayer
        }   
        
        public Game(GameState state, int playerIndex)
        {
            InitializeComponent();
            board = new Board();
            pictureBox = new PictureBox();
            gameState = state;

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
            this.playerIndex = playerIndex;
            messageReceiver.DoWork += messageReceiver_DoWork;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void messageReceiver_DoWork(object sender, DoWorkEventArgs e)
        {
            ReceiveMessage();
            Debug.WriteLine("message receiver "+data);
        }

        public void InitalizeGame()
        {
            if (playerIndex == 1)
            {
                if (gameState == GameState.AgainstPC)
                {
                    GamePlay.whoStartsTheGame();
                    GamePlay.createPlayers();
                    SetPlayerName(GamePlay.player1.getName(), "PC");
                }
                else if (gameState == GameState.AgainstPlayer)//host
                {
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
                        MessageBox.Show("Se asteapta conectarea jucatorilor");
                        socket = listener.Accept();
                        MessageBox.Show("S-a conectat");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                    messageReceiver.RunWorkerAsync();//background worker executa metoda lui
                    while(data.CompareTo("")==0)
                    {

                    }
                    //Debug.WriteLine(data);
                    string[] val = new string[2];
                    val = data.Split(' ');
                    GamePlay.setMyTurn1(int.Parse(val[0].Trim()));
                    GamePlay.setMyTurn2(int.Parse(val[1].Trim()));
                    GamePlay.createPlayers();
                    SetPlayerName(GamePlay.player1.getName(), "");
                    data = "";
                }
            }
            else
            {
                if (playerIndex == 2 && gameState == GameState.AgainstPlayer)//client
                {
                    try
                    {
                        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                        IPEndPoint remoteEP = new IPEndPoint(ipAddress, int.Parse("8888"));
                        socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        // Connect the socket to the remote endpoint. Catch any errors.
                        try
                        {
                            // Connect to Remote EndPoint
                            socket.Connect(remoteEP);
                            Debug.WriteLine("Socket connected to {0}", socket.RemoteEndPoint.ToString());
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
                    GamePlay.whoStartsTheGame();
                    GamePlay.createPlayers();
                    socket.Send(Encoding.ASCII.GetBytes(GamePlay.myTurn1.ToString() + " " + GamePlay.myTurn2.ToString()));
                    SetPlayerName(GamePlay.player2.getName(), "");
                }
            }
            SetPlayerTurn(GamePlay.myTurn1, GamePlay.myTurn2);
            SetTextFromTextBox("Playerul " + GamePlay.player1.getName() + " are randul " + GamePlay.player1.myTurn + " si culoarea " + (GamePlay.player1.myColor == true ? "alb" : "negru") + "\r\n");
            SetTextFromTextBox("Playerul " + GamePlay.player2.getName() + " are randul " + GamePlay.player2.myTurn + " si culoarea " + (GamePlay.player2.myColor == true ? "alb" : "negru") + "\r\n");
        }
        public void PlacingAgainstPlayer(object sender)
        {
            string currentPlayerName = labelName1.Text;
            Player activePlayer = GamePlay.getActivePlayer();
            string activePlayerName = activePlayer.getName();
            Player inactivePlayer = GamePlay.getInactivePlayer();
            string inactivePlayerName = inactivePlayer.getName();
            if (inactivePlayer.myState == Player.PlayerState.Placing || activePlayer.myState == Player.PlayerState.Placing)
            {
                Debug.WriteLine(GamePlay.getActivePlayer().getName() + " player care poate pune piesa");
                Debug.WriteLine(currentPlayerName + " player care vrea sa puna piesa");
                if (currentPlayerName.CompareTo(activePlayerName) == 0)
                {
                    var panel = sender as Panel;
                    if (null != panel)
                    {
                        if (activePlayer.myHandCells.Count() > 0)
                        {
                            if (board.cells.ElementAt(Int32.Parse(panel.Name) - 1).State == CellState.Empty)
                            {
                                board.panels[Int32.Parse(panel.Name) - 1].BackgroundImage = GamePlay.getActivePlayer().getMyColor();
                                bool color = activePlayer.myColor;
                                board.cells.ElementAt(Int32.Parse(panel.Name) - 1).State = color == true ? CellState.WhiteOccupied : CellState.BlackOccupied;
                                board.updateCells();
                                GamePlay.playerTurn(activePlayer, inactivePlayer, activePlayer.myHandCells.Count);
                                SetTextFromTextBox("Mai ai " + activePlayer.myHandCells.Count + " piese de pus" + "\r\nAcum e randul la " + inactivePlayerName + "\r\n");
                                socket.Send(Encoding.ASCII.GetBytes((Int32.Parse(panel.Name) - 1).ToString()));
                                if (activePlayer.myHandCells.Count == 0)
                                {
                                    MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                                    activePlayer.myState = Player.PlayerState.Moving;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Loc deja ocupat de alta piesa");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                        }
                    }
                }
                else
                {
                    if (activePlayer.myHandCells.Count() > 0)
                    {
                        messageReceiver.RunWorkerAsync();
                        while (data.CompareTo("") == 0)
                        {

                        }
                        int panelIndex = int.Parse(data);
                        board.panels[panelIndex].BackgroundImage = GamePlay.getActivePlayer().getMyColor();
                        bool color = activePlayer.myColor;
                        board.cells.ElementAt(panelIndex).State = color == true ? CellState.WhiteOccupied : CellState.BlackOccupied;
                        board.updateCells();
                        GamePlay.playerTurn(activePlayer, inactivePlayer, activePlayer.myHandCells.Count);
                        data = "";
                    }
                }
            }
        }
        public void PlacingAgainstPC(object sender)
        {

        }
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Game_MouseClick(sender, e);
        }

        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            if(this.gameState == GameState.AgainstPlayer)
            {
                PlacingAgainstPlayer(sender);
            }
            else
            {
                PlacingAgainstPC(sender);
            }
        }

        private void buttonSurrender_Click(object sender, EventArgs e)
        {
            messageReceiver.WorkerSupportsCancellation = true;  
            messageReceiver.CancelAsync();
            this.Close();
        }

        public void SetTextFromTextBox(string newText)
        {
            this.textBoxGameLog.AppendText(newText);
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
        public void ReceiveMessage()
        {
            byte[] bytes = null;
            bytes = new byte[1024];
            int bytesRec = socket.Receive(bytes);
            Game.data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            //Debug.WriteLine(data);
        }
    }
}
