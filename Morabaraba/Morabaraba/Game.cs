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
        private static PictureBox pictureBox;
        private GameState gameState;
        private static Socket socket;//serverSocket vs clientSocket
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
        public void DecodeMessage()
        {
            string[] mess = data.Split(' ');
            //Debug.WriteLine(mess[0]);
            int panelIndex = int.Parse(mess[0].Trim());
            string action = mess[1].Trim();
            string switchTurn = "";
            if (action.EndsWith("2"))
            {
                switchTurn = "2";
                action = action.Substring(0, action.Length - 1);
                //Debug.WriteLine(action+ "hras");
            }
            BoardCell cell = board.GetCells().ElementAt(panelIndex);
            switch (action)
            {
                case "PartOfThree":
                    bool partOfThree = Convert.ToBoolean(mess[2].Trim());
                    int poz = GamePlay.GetActivePlayer().GetMyBoardCells().IndexOf(cell);
                    if ( GamePlay.GetActivePlayer().GetMyBoardCells().Contains(cell))
                    {
                        GamePlay.GetActivePlayer().GetMyBoardCells()[poz].SetPartOfThree(partOfThree);
                    }
                    else
                    {
                        GamePlay.GetActivePlayer().GetMyBoardCells().Add(board.GetCells().ElementAt(panelIndex));
                        GamePlay.GetActivePlayer().GetMyBoardCells()[GamePlay.GetActivePlayer().GetMyBoardCells().Count-1].SetPartOfThree(partOfThree);
                    }
                    switchTurn = "2";
                    break;
                case "Placing":
                    bool color = GamePlay.GetActivePlayer().GetMyColor();
                    board.GetCells().ElementAt(panelIndex).SetState(color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
                    board.UpdateCells();
                    if (!GamePlay.GetActivePlayer().GetMyBoardCells().Contains(cell))
                    {
                        GamePlay.GetActivePlayer().GetMyBoardCells().Add(board.GetCells().ElementAt(panelIndex));
                    }
                    break;
                case "Taking":
                    BoardCell cellRemove = board.GetCells().ElementAt(panelIndex);
                    GamePlay.GetInactivePlayer().GetMyBoardCells().Remove(cellRemove);
                    board.GetCells().ElementAt(panelIndex).SetState(BoardCell.CellState.Empty);
                    board.UpdateCells();
                    break;
            }
            if (switchTurn.CompareTo("") == 0) 
            {
                GamePlay.PlayerTurn(GamePlay.GetActivePlayer(), GamePlay.GetInactivePlayer(), GamePlay.GetActivePlayer().GetMyHandCells().Count);
            }
            if (mess.Length > 3)
            {
                data = (data.Substring(data.IndexOf("true")+4)).Trim();
                //Console.WriteLine(data);
                DecodeMessage();
            }
        }
        public void PlacingAgainstPlayer(object sender, string currentPlayerName)
        {
            Player activePlayer = GamePlay.GetActivePlayer();
            Player inactivePlayer = GamePlay.GetInactivePlayer();
            string activePlayerName = activePlayer.GetMyName();
            string inactivePlayerName = inactivePlayer.GetMyName();
            {
                //Debug.WriteLine(GamePlay.GetActivePlayer().GetMyName() + " player care poate pune piesa");
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
                                activePlayer.GetMyBoardCells().Add(board.GetCells().ElementAt(Int32.Parse(panel.Name)));
                                bool color = activePlayer.GetMyColor();
                                board.GetCells().ElementAt(Int32.Parse(panel.Name)).SetState( color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
                                board.UpdateCells();
                                if (activePlayer.GetMyBoardCells().Count() > 2)
                                {
                                    GamePlay.CheckForMill(activePlayer, board.GetCells().ElementAt(Int32.Parse(panel.Name)));
                                    for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
                                    {
                                        if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                                        {
                                            MessageBox.Show("Felicitari ati facut o moara! Luati o piesa a adversarului.");
                                            activePlayer.SetMyState(Player.PlayerState.Taking);
                                        }
                                    }
                                }
                                if (activePlayer.GetMyState() != Player.PlayerState.Taking)
                                {
                                    GamePlay.PlayerTurn(activePlayer, inactivePlayer, activePlayer.GetMyHandCells().Count);
                                    SetTextFromTextBox("Mai ai " + activePlayer.GetMyHandCells().Count + " piese de pus" + "\r\nAcum e randul la " + inactivePlayerName + "\r\n");
                                    socket.Send(Encoding.ASCII.GetBytes((Int32.Parse(panel.Name)).ToString() + " Placing"));
                                }
                                else
                                {
                                    socket.Send(Encoding.ASCII.GetBytes((Int32.Parse(panel.Name)).ToString() + " Placing2"));
                                }
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
                    if (activePlayer.GetMyHandCells().Count() == 0)
                    {
                        MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                    }
                }
            }
        }
        public void TakeCow(object sender)
        {
            String inactivePlayerName = GamePlay.GetInactivePlayer().GetMyName();
            MessageBox.Show("Ia o piesa!");
            Player activePlayer = GamePlay.GetActivePlayer();
            var panel = sender as Panel;
            Mill mill1 = new Mill();
            int ctMill = GamePlay.NewPlayerMill(GamePlay.GetActivePlayer());
            if (null != panel)
            {
                Debug.WriteLine(board.GetCells().ElementAt(int.Parse(panel.Name)).GetPartOfThree());
                Debug.WriteLine(GamePlay.GetInactivePlayer().GetMyName());
                for(int i=0; i < GamePlay.GetInactivePlayer().GetMyBoardCells().Count; i++)
                {
                    Debug.WriteLine(GamePlay.GetInactivePlayer().GetMyBoardCells()[i].GetPartOfThree()+ " "+ GamePlay.GetInactivePlayer().GetMyBoardCells()[i].GetId());
                }
                if ((board.GetCells().ElementAt(int.Parse(panel.Name)).GetState() == (GamePlay.GetInactivePlayer().GetMyColor() == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied)) && (!(board.GetCells().ElementAt(int.Parse(panel.Name)).GetPartOfThree()) || GamePlay.AllPlayerCellsInMills()))
                {
                    BoardCell cellRemove = board.GetCells().ElementAt(int.Parse(panel.Name));
                    if (GamePlay.AllPlayerCellsInMills())
                    {
                        Debug.WriteLine("intra Marcus?");
                        GamePlay.DestroyMill(cellRemove);
                    }
                    GamePlay.GetInactivePlayer().GetMyBoardCells().Remove(cellRemove);
                    board.GetCells().ElementAt(int.Parse(panel.Name)).SetState(BoardCell.CellState.Empty);
                    board.UpdateCells();
                    if (ctMill != 1)
                    {
                        activePlayer.SetMyState(Player.PlayerState.Taking);
                        socket.Send(Encoding.ASCII.GetBytes((int.Parse(panel.Name)).ToString() + " Taking2"));
                    }
                    else
                    {
                        MessageBox.Show("E randul adversarului");
                        if (activePlayer.GetMyHandCells().Count == 0)
                        {
                            activePlayer.SetMyState(Player.PlayerState.Moving);
                        }
                        else if (activePlayer.GetMyBoardCells().Count == 3 && activePlayer.GetMyHandCells().Count == 0)
                        {
                            activePlayer.SetMyState(Player.PlayerState.Flying);
                        }
                        else
                        {
                            activePlayer.SetMyState(Player.PlayerState.Placing);
                        }
                        GamePlay.PlayerTurn(activePlayer, GamePlay.GetInactivePlayer(), activePlayer.GetMyHandCells().Count);
                        SetTextFromTextBox("Mai ai " + activePlayer.GetMyHandCells().Count + " piese de pus" + "\r\nAcum e randul la " + inactivePlayerName + "\r\n");
                        SetTextFromTextBox(activePlayer.GetMyState().ToString() + "\r\n");
                        if (activePlayer.GetMyHandCells().Count == 0)
                        {
                            MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                            activePlayer.SetMyState(Player.PlayerState.Moving);
                        }
                        socket.Send(Encoding.ASCII.GetBytes((int.Parse(panel.Name)).ToString() + " Taking"));
                    }
                    for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
                    {
                        if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                        {
                            mill1 = activePlayer.GetMyMills()[i];
                            activePlayer.GetMyMills()[i].SetIsNew(false);
                            break;
                        }
                    }
                    ctMill = GamePlay.NewPlayerMill(activePlayer);
                }
                else
                {
                    MessageBox.Show("Nu poti lua piese dintr-o moara sau trebuie sa selectezi o piesa a adversarului!");
                }
            }
            else
            {
                MessageBox.Show("Alegeti o piesa valida");
            }
            if(GamePlay.GetInactivePlayer().GetMyBoardCells().Count < 3 && GamePlay.GetInactivePlayer().GetMyHandCells().Count==0)
            {
                Debug.WriteLine("Joc terminat" + activePlayer.GetMyName() + " e castigator");
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
                string currentPlayerName = labelName1.Text;
                Player activePlayer = GamePlay.GetActivePlayer();
                Player inactivePlayer = GamePlay.GetInactivePlayer();
                Player.PlayerState playerState = activePlayer.GetMyState();
                if(currentPlayerName.CompareTo(inactivePlayer.GetMyName()) == 0)
                {
                    FreezeBoard();
                    EnabledPanelContents(false);
                    Application.DoEvents();
                    messageReceiver.RunWorkerAsync();
                    while (data.CompareTo("") == 0)
                    {

                    }
                    DecodeMessage();
                    data = "";
                    return;
                }
                else
                {
                    UnfreezeBoard();
                    EnabledPanelContents(true);
                }
                switch (playerState)
                {
                    case Player.PlayerState.Placing:
                        PlacingAgainstPlayer(sender, currentPlayerName);
                        break;
                    case Player.PlayerState.Taking:
                        TakeCow(sender);
                        break;
                    case Player.PlayerState.Moving:
                        //to be implemented
                        break;
                    case Player.PlayerState.Flying:
                        //to be implemented
                        break;
                }
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
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
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
        }
        public static void FreezeBoard()
        {
            pictureBox.Enabled = false;

        }
        public static void UnfreezeBoard()
        {
            pictureBox.Enabled = false;

        }
        public static Socket GetSocket()
        {
            return socket;
        }
        public static Board GetBoard()
        {
            return board;
        }
        private void EnabledPanelContents( bool enabled )
        {
            for(int i=0;i<board.GetPanels().Length;i++)
            {
                foreach (Control ctrl in board.GetPanels()[i].Controls)
                {
                    ctrl.Enabled = enabled;
                }
            }
        }
    }
}
