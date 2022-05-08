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
        private static Board board;
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

            for (int i = 0; i < board.GetPanels().Length; i++)
            {
                board.GetPanels()[i].MouseClick += Game_MouseClick;
                this.Controls.Add(board.GetPanels()[i]);
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
                    GamePlay.WhoStartsTheGame();
                    GamePlay.CreatePlayers();
                    SetPlayerName(GamePlay.GetPlayer1().GetMyName(), "PC");
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
                    GamePlay.SetMyTurn1(int.Parse(val[0].Trim()));
                    GamePlay.SetMyTurn2(int.Parse(val[1].Trim()));
                    GamePlay.CreatePlayers();
                    SetPlayerName(GamePlay.GetPlayer1().GetMyName(), "");
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
                    GamePlay.WhoStartsTheGame();
                    GamePlay.CreatePlayers();
                    socket.Send(Encoding.ASCII.GetBytes(GamePlay.GetMyTurn1().ToString() + " " + GamePlay.GetMyTurn2().ToString()));
                    SetPlayerName(GamePlay.GetPlayer2().GetMyName(), "");
                }
            }
            SetPlayerTurn(GamePlay.GetMyTurn1(), GamePlay.GetMyTurn2());
            SetTextFromTextBox("Playerul " + GamePlay.GetPlayer1().GetMyName() + " are randul " + GamePlay.GetPlayer1().GetMyTurn() + " si culoarea " + (GamePlay.GetPlayer1().GetMyColor() == true ? "alb" : "negru") + "\r\n");
            SetTextFromTextBox("Playerul " + GamePlay.GetPlayer2().GetMyName() + " are randul " + GamePlay.GetPlayer2().GetMyTurn() + " si culoarea " + (GamePlay.GetPlayer2().GetMyColor() == true ? "alb" : "negru") + "\r\n");
        }
        public void PlacingAgainstPlayer(object sender)
        {
            string currentPlayerName = labelName1.Text;
            Player activePlayer = GamePlay.GetActivePlayer();
            string activePlayerName = activePlayer.GetMyName();
            Player inactivePlayer = GamePlay.GetInactivePlayer();
            string inactivePlayerName = inactivePlayer.GetMyName();
            if (inactivePlayer.GetMyState() == Player.PlayerState.Placing || activePlayer.GetMyState() == Player.PlayerState.Placing)
            {
                //Debug.WriteLine(GamePlay.getActivePlayer().GetMyName() + " player care poate pune piesa");
                //Debug.WriteLine(currentPlayerName + " player care vrea sa puna piesa");
                if (currentPlayerName.CompareTo(activePlayerName) == 0)
                {
                    var panel = sender as Panel;
                    if (null != panel)
                    {
                        if (activePlayer.GetMyHandCells().Count() > 0)
                        {
                            if (board.GetCells().ElementAt(Int32.Parse(panel.Name)).GetState() == BoardCell.CellState.Empty)
                            {
                                board.GetPanels()[Int32.Parse(panel.Name)].BackgroundImage = GamePlay.GetActivePlayer().DrawMyColor();
                                activePlayer.GetMyBoardCells().Add(board.GetCells().ElementAt(Int32.Parse(panel.Name)));//piesele mele de pe board
                                bool color = activePlayer.GetMyColor();
                                board.GetCells().ElementAt(Int32.Parse(panel.Name)).SetState( color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
                                board.UpdateCells();
                                if(activePlayer.GetMyBoardCells().Count() > 2)
                                {
                                    //Debug.WriteLine(activePlayer.GetMyName() +" "+ activePlayer.GetMyBoardCells().Count());
                                    GamePlay.CheckForMill(activePlayer, board.GetCells().ElementAt(Int32.Parse(panel.Name)));//verificare moara
                                    for(int i=0; i < activePlayer.GetMyMills().Count(); i++)
                                    {
                                        if(GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                                        {
                                            MessageBox.Show("Felicitari ati facut o moara! Luati o piesa a adversarului.");
                                            activePlayer.GetMyMills()[i].SetIsNew(false);// nu mai e noua 
                                        }
                                    }
                                }
                                GamePlay.PlayerTurn(activePlayer, inactivePlayer, activePlayer.GetMyHandCells().Count);
                                SetTextFromTextBox("Mai ai " + activePlayer.GetMyHandCells().Count + " piese de pus" + "\r\nAcum e randul la " + inactivePlayerName + "\r\n");
                                socket.Send(Encoding.ASCII.GetBytes((Int32.Parse(panel.Name)).ToString()));

                                if (activePlayer.GetMyHandCells().Count == 0)
                                {
                                    MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                                    activePlayer.SetMyState(Player.PlayerState.Moving);
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
                    if (activePlayer.GetMyHandCells().Count() > 0)
                    {
                        messageReceiver.RunWorkerAsync();
                        while (data.CompareTo("") == 0)
                        {

                        }
                        int panelIndex = int.Parse(data);
                        board.GetPanels()[panelIndex].BackgroundImage = GamePlay.GetActivePlayer().DrawMyColor();
                        bool color = activePlayer.GetMyColor();
                        board.GetCells().ElementAt(panelIndex).SetState( color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
                        board.UpdateCells();
                        GamePlay.PlayerTurn(activePlayer, inactivePlayer, activePlayer.GetMyHandCells().Count);
                        data = "";
                    }
                    else
                    {
                        MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
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
                //CheckForMill -> Taking
                //MovingAgainstPlayer
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
        public static Board GetBoard()
        {
            return board;
        }
    }
}
