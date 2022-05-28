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
        private static GameState gameState;
        private static Socket socket;//serverSocket vs clientSocket
        private int playerIndex;
        private BackgroundWorker messageReceiver = new BackgroundWorker();
        private static string data="";
        private static List<BoardCell> posibilities = new List<BoardCell>();
        private static BoardCell selectedCell;
        private static int counter = 5;
        AI aiPlayer;
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
            pictureBox.Enabled = false;

            for (int i = 0; i < board.GetPanels().Length; i++)
            {
                board.GetPanels()[i].MouseClick += Game_MouseClick;
                board.GetPanels()[i].MouseDown += Game_MouseDown;
                board.GetPanels()[i].DragOver += Game_DragOver;
                board.GetPanels()[i].DragDrop += Game_DragDrop;
                
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
            Debug.WriteLine("------Message Receiver------\r\n" + data);
        }

        public void InitalizeGame()
        {
            if (playerIndex == 1)
            {
                if (gameState == GameState.AgainstPC)
                {
                    aiPlayer = new AI();
                    GamePlay.WhoStartsTheGame();
                    GamePlay.CreatePlayers();
                    SetPlayerName(GamePlay.GetPlayer1().GetMyName(), "PC");
                    GamePlay.GetPlayer2().SetMyName("PC");
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
                    string[] val = new string[2];
                    val = data.Split(' ');
                    GamePlay.SetMyTurn1(int.Parse(val[0].Trim()));
                    GamePlay.SetMyTurn2(int.Parse(val[1].Trim()));
                    GamePlay.CreatePlayers();
                    SetPlayerName(GamePlay.GetPlayer1().GetMyName(), GamePlay.GetPlayer2().GetMyName());
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
                    SetPlayerName(GamePlay.GetPlayer2().GetMyName(), GamePlay.GetPlayer1().GetMyName());
                }
            }
            SetPlayerTurn(GamePlay.GetMyTurn1(), GamePlay.GetMyTurn2());
            SetTextFromTextBox("Playerul " + GamePlay.GetPlayer1().GetMyName() + " are randul " + GamePlay.GetPlayer1().GetMyTurn() + " si culoarea " + (GamePlay.GetPlayer1().GetMyColor() == true ? "alb" : "negru") + " " + GamePlay.GetPlayer1().GetMyHandCells().Count + " piese\r\n");
            SetTextFromTextBox("Playerul " + GamePlay.GetPlayer2().GetMyName() + " are randul " + GamePlay.GetPlayer2().GetMyTurn() + " si culoarea " + (GamePlay.GetPlayer2().GetMyColor() == true ? "alb" : "negru") + " " + GamePlay.GetPlayer2().GetMyHandCells().Count + " piese\r\n");

            if (GamePlay.GetActivePlayer().GetMyName() == "PC")
            {
                aiPlayer.PlaceRandomPiece();
                GamePlay.SwitchTurn();
            }
        }

        public void DecodeMessage()
        {
            data = data.Trim();
            string[] mess = data.Split(' ');
            int panelIndex = int.Parse(mess[0].Trim());
            string action = mess[1].Trim(); 
            string switchTurn = "";
            if (action.EndsWith("2"))
            {
                switchTurn = "2";
                action = action.Substring(0, action.Length - 1);
            }
            BoardCell cell = board.GetCells().ElementAt(panelIndex);
            switch (action)
            {
                case "PartOfThree":
                    bool partOfThree = Convert.ToBoolean(mess[2].Trim());
                    int poz = GamePlay.GetActivePlayer().GetMyBoardCells().IndexOf(cell);
                    int poz1 = GamePlay.GetInactivePlayer().GetMyBoardCells().IndexOf(cell);
                    if (GamePlay.GetActivePlayer().GetMyBoardCells().Contains(cell))
                    {
                        GamePlay.GetActivePlayer().GetMyBoardCells()[poz].SetPartOfThree(partOfThree);
                    }
                    else
                    {
                        if (GamePlay.GetInactivePlayer().GetMyBoardCells().Contains(cell))
                        {
                            GamePlay.GetInactivePlayer().GetMyBoardCells()[poz1].SetPartOfThree(partOfThree);
                        }
                        else
                        {
                            GamePlay.GetActivePlayer().GetMyBoardCells().Add(cell);
                            GamePlay.GetActivePlayer().GetMyBoardCells()[GamePlay.GetActivePlayer().GetMyBoardCells().Count - 1].SetPartOfThree(partOfThree);
                        }
                    }
                    if (partOfThree == true)
                    {
                        GamePlay.GetMill().GetMillCells().Add(cell);
                    }
                    if(GamePlay.GetMill().GetMillCells().Count>2)
                    {
                        for(int k=0;k<GamePlay.GetActivePlayer().GetMyMills().Length; k++)
                        {
                            if(GamePlay.GetActivePlayer().GetMyMills()[k].GetIsNew()==false)
                            {
                                Debug.WriteLine("A facut moara din " + GamePlay.GetMill().GetMillCells().ElementAt(0).GetId() + " " + GamePlay.GetMill().GetMillCells().ElementAt(1).GetId() + " " + GamePlay.GetMill().GetMillCells().ElementAt(2).GetId());
                                GamePlay.GetActivePlayer().GetMyMills()[k] = new Mill(GamePlay.GetMill().GetMillCells().ElementAt(0), GamePlay.GetMill().GetMillCells().ElementAt(1), GamePlay.GetMill().GetMillCells().ElementAt(2));
                                break;
                            }
                        }
                        GamePlay.GetMill().GetMillCells().Clear(); 
                    }
                    if (mess.Length > 3)
                    {
                        switchTurn = "2";
                    }
                    else
                    {
                        switchTurn = "";
                    }
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
                    if (mess.Length != 3)
                    {
                        switchTurn = "2";
                    }
                    break;
                case "Moving":
                    int target = int.Parse(mess[2].Trim());
                    if(panelIndex != target)
                    { 
                        board.GetCells().ElementAt(cell.GetId()).SetState(BoardCell.CellState.Empty);
                        color = GamePlay.GetActivePlayer().GetMyColor();
                        board.GetCells().ElementAt(target).SetState(color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
                        board.UpdateCells();
                        int index = GamePlay.GetActivePlayer().GetMyBoardCells().FindIndex(x => x.GetId() == board.GetCells().ElementAt(panelIndex).GetId());
                        GamePlay.GetActivePlayer().GetMyBoardCells()[index] = board.GetCells().ElementAt(target);
                    }
                    else
                    {
                        switchTurn = "2";
                    }
                    if ( mess.Length != 3)
                    {
                        switchTurn = "2";
                    }
                    break;
                case "FlyingState":
                    GamePlay.GetInactivePlayer().SetMyState(Player.PlayerState.Flying);
                    if(mess.Length != 3)
                    {
                        switchTurn= "2";
                    }
                    break;
                case "Victory":
                    MessageBox.Show( mess[2] + " a castigat meciul!");
                    System.Windows.Forms.Application.Exit();
                    break;
                case "Draw":
                    MessageBox.Show("Meciul s-a terminat la egalitate!");
                    System.Windows.Forms.Application.Exit();
                    break;
                case "Surrender":
                    MessageBox.Show( "player" + mess[0]+" s-a predat, " + this.labelName1.Text + " e castigatorul");
                    System.Windows.Forms.Application.Exit();
                    break;
                case "Counter":
                    counter = panelIndex;
                    Debug.WriteLine(counter);
                    if (mess.Length != 3)
                    {
                        switchTurn = "2";
                    }
                    break;
            }
            if (switchTurn.CompareTo("") == 0) 
            {
                GamePlay.SwitchTurn();
            }
            if (mess.Length > 3)
            {
                data = (data.Substring(data.IndexOf(mess[1]) + mess[1].Length + mess[2].Length+1).Trim());
                DecodeMessage();
            }
        }

        public void PlaceCow(object sender, string currentPlayerName)
        {
            Player activePlayer = GamePlay.GetActivePlayer();
            Player inactivePlayer = GamePlay.GetInactivePlayer();
            string activePlayerName = activePlayer.GetMyName();
            string inactivePlayerName = inactivePlayer.GetMyName();
            {
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
                                GamePlay.CheckForMill(activePlayer, board.GetCells().ElementAt(Int32.Parse(panel.Name)));
                                for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
                                {
                                    if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                                    {
                                        MessageBox.Show("Felicitari ati facut o moara! Luati o piesa a adversarului.");
                                        activePlayer.SetMyState(Player.PlayerState.Taking);
                                    }
                                }
                                if (activePlayer.GetMyState() != Player.PlayerState.Taking)
                                {
                                    GamePlay.SwitchTurn();
                                    SetTextFromTextBox("Mai ai " + activePlayer.GetMyHandCells().Count + " piese de pus" + "\r\nAcum e randul la " + inactivePlayerName + "\r\n");
                                    if(gameState == GameState.AgainstPlayer)
                                    {
                                        socket.Send(Encoding.ASCII.GetBytes((Int32.Parse(panel.Name)).ToString() + " Placing " +"0 \r\n"));
                                    }
                                    if (activePlayer.GetMyHandCells().Count == 0)
                                    {
                                        MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare");
                                        activePlayer.SetMyState(Player.PlayerState.Moving);
                                    }
                                }
                                else
                                {
                                    if (gameState == GameState.AgainstPlayer)
                                    {
                                        socket.Send(Encoding.ASCII.GetBytes((Int32.Parse(panel.Name)).ToString() + " Placing2 " +"0 \r\n"));
                                    }
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
            string inactivePlayerName = GamePlay.GetInactivePlayer().GetMyName();
            MessageBox.Show("Ia o piesa!");
            Player activePlayer = GamePlay.GetActivePlayer();
            var panel = sender as Panel;
            Mill mill1 = new Mill();
            int ctMill = GamePlay.NewPlayerMillCount(GamePlay.GetActivePlayer());
            if (null != panel)
            {
                if ((board.GetCells().ElementAt(int.Parse(panel.Name)).GetState() == (GamePlay.GetInactivePlayer().GetMyColor() == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied)) && (!(board.GetCells().ElementAt(int.Parse(panel.Name)).GetPartOfThree()) || GamePlay.AllPlayerCellsInMills()))
                {
                    BoardCell cellRemove = board.GetCells().ElementAt(int.Parse(panel.Name));
                    if (GamePlay.AllPlayerCellsInMills())
                    {
                        GamePlay.DestroyMill(cellRemove);
                    }
                    GamePlay.GetInactivePlayer().GetMyBoardCells().Remove(cellRemove);
                    board.GetCells().ElementAt(int.Parse(panel.Name)).SetState(BoardCell.CellState.Empty);
                    board.UpdateCells();
                    if (GamePlay.GetInactivePlayer().GetMyBoardCells().Count == 3 && GamePlay.GetInactivePlayer().GetMyHandCells().Count == 0)
                    {
                        GamePlay.GetInactivePlayer().SetMyState(Player.PlayerState.Flying);
                        if (gameState == GameState.AgainstPlayer)
                        {
                            socket.Send(Encoding.ASCII.GetBytes(0 + " FlyingState " + "0 \r\n"));
                        }
                    }
                    if (GamePlay.GetInactivePlayer().GetMyBoardCells().Count < 3 && GamePlay.GetInactivePlayer().GetMyHandCells().Count == 0)
                    {
                        MessageBox.Show("Joc terminat " + activePlayer.GetMyName() + " e castigator");
                        if (gameState == GameState.AgainstPlayer)
                        {
                            socket.Send(Encoding.ASCII.GetBytes(0 + " Victory " + activePlayer.GetMyName() + " \r\n"));
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                        }
                        messageReceiver.WorkerSupportsCancellation = true;
                        messageReceiver.CancelAsync();
                        System.Windows.Forms.Application.Exit();
                        return;
                    }
                    if (ctMill != 1)
                    {
                        activePlayer.SetMyState(Player.PlayerState.Taking);
                        if (gameState == GameState.AgainstPlayer)
                        {
                            socket.Send(Encoding.ASCII.GetBytes((int.Parse(panel.Name)).ToString() + " Taking2 " + "0 \r\n"));
                        }
                    }
                    else
                    {
                        //moving
                        if (activePlayer.GetMyHandCells().Count == 0 && activePlayer.GetMyBoardCells().Count > 3)
                        {
                            activePlayer.SetMyState(Player.PlayerState.Moving);
                        }
                        //flying
                        if (activePlayer.GetMyBoardCells().Count == 3 && activePlayer.GetMyHandCells().Count == 0)
                        {
                            activePlayer.SetMyState(Player.PlayerState.Flying);
                        }
                        //placing
                        if (activePlayer.GetMyHandCells().Count > 0)
                        {
                            activePlayer.SetMyState(Player.PlayerState.Placing);
                            GamePlay.SwitchTurn();
                            SetTextFromTextBox("Mai ai " + activePlayer.GetMyHandCells().Count + " piese de pus" + "\r\nAcum e randul la " + inactivePlayerName + "\r\n");
                            if (activePlayer.GetMyHandCells().Count == 0)
                            {
                                if (activePlayer.GetMyBoardCells().Count == 3 && activePlayer.GetMyHandCells().Count == 0)
                                {
                                    activePlayer.SetMyState(Player.PlayerState.Flying);
                                }
                                else
                                {
                                    MessageBox.Show("Ati terminat toate piesele. Treci la etapa de mutare!");
                                    activePlayer.SetMyState(Player.PlayerState.Moving);
                                }
                            }
                        }
                        else
                        {
                            GamePlay.SwitchTurn();
                        }
                        MessageBox.Show("E randul adversarului");
                        if (gameState == GameState.AgainstPlayer)
                        {
                            socket.Send(Encoding.ASCII.GetBytes((int.Parse(panel.Name)).ToString() + " Taking " +"0 \r\n"));
                        }
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
                    ctMill = GamePlay.NewPlayerMillCount(activePlayer);
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
        }

        public static void VerifyCowInMill(object sender)
        {
            Player activePlayer = GamePlay.GetActivePlayer();
            Panel panel = sender as Panel;
            if (panel != null)
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
        }

        public static void VerifyIfDestroyedMill(int index, object sender)
        {
            Player activePlayer = GamePlay.GetActivePlayer();
            for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
            {
                if (activePlayer.GetMyMills()[i].GetMillCells() != null)
                {
                    if (activePlayer.GetMyMills()[i].GetMillCells().Contains(board.GetCells().ElementAt(index)))
                    {
                        GamePlay.SendMillMess(activePlayer.GetMyMills()[i], "false");
                        activePlayer.GetMyMills()[i] = new Mill();
                    }
                }
            }
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            Game_MouseClick(sender, e);
        }

        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            string currentPlayerName = labelName1.Text;
            Player activePlayer = GamePlay.GetActivePlayer();
            Player inactivePlayer = GamePlay.GetInactivePlayer();
            Player.PlayerState playerState = activePlayer.GetMyState();
            if(currentPlayerName.CompareTo(inactivePlayer.GetMyName()) == 0)
            {
                EnabledPanelContents(false);
                if (gameState == GameState.AgainstPlayer)
                {
                    Application.DoEvents();
                    messageReceiver.RunWorkerAsync();
                    while (data.CompareTo("") == 0)
                    {

                    }
                    DecodeMessage();
                    data = "";
                    return;
                }
            }
            else
            {
                EnabledPanelContents(true);
            }
            switch (playerState)
            {
                case Player.PlayerState.Placing:
                    PlaceCow(sender, currentPlayerName);
                    break;
                case Player.PlayerState.Taking:
                    TakeCow(sender);
                    break;
            }
        }

        private void buttonSurrender_Click(object sender, EventArgs e)
        {
            if(gameState == GameState.AgainstPlayer)
            {     
                char last = this.labelName1.Text[this.labelName1.Text.Length - 1];
                socket.Send(Encoding.ASCII.GetBytes(last.ToString() + " Surrender " +"0 \r\n"));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            MessageBox.Show(this.labelName1.Text + " s-a predat," + this.labelName2.Text + " e castigatorul");
            System.Windows.Forms.Application.Exit();
            messageReceiver.WorkerSupportsCancellation = true;  
            messageReceiver.CancelAsync();
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

        public static Socket GetSocket()
        {
            return socket;
        }

        public static Board GetBoard()
        {
            return board;
        }

        public static GameState GetState()
        {
            return gameState;
        }

        public static void EnabledPanelContents( bool enabled )
        {
            for(int i=0;i<board.GetPanels().Length;i++)
            {
                foreach (Control ctrl in board.GetPanels()[i].Controls)
                {
                    ctrl.Enabled = enabled;
                }
            }
        }

        private void Game_MouseDown(object sender, MouseEventArgs e)
        {
            Player activePlayer = GamePlay.GetActivePlayer();
            string currentPlayerName = labelName1.Text;
            if ((activePlayer.GetMyState() == Player.PlayerState.Moving || activePlayer.GetMyState() == Player.PlayerState.Flying) && activePlayer.GetMyName().Equals(currentPlayerName))
            {
                Panel panel = sender as Panel;
                if (panel != null)
                {
                    if (panel != null && true == activePlayer.GetMyBoardCells().Contains(board.GetCells().ElementAt(int.Parse(panel.Name))))
                    {
                        if (GamePlay.CheckIfCanMove(activePlayer))
                        {
                            panel.DoDragDrop(board.GetCells().ElementAt(int.Parse(panel.Name)), DragDropEffects.Move);
                            if (posibilities.Count > 0)
                            {
                                int index = activePlayer.GetMyBoardCells().FindIndex(x => x.GetId() == board.GetCells().ElementAt(int.Parse(panel.Name)).GetId());
                                activePlayer.GetMyBoardCells()[index] = board.GetCells().ElementAt(posibilities.ElementAt(posibilities.Count - 1).GetId());
                                Panel panelToVerify = board.GetPanels()[board.GetCells().ElementAt(posibilities.ElementAt(posibilities.Count - 1).GetId()).GetId()];
                                Player.PlayerState playerState = GamePlay.GetActivePlayer().GetMyState();
                                Player.PlayerState enemyState = GamePlay.GetInactivePlayer().GetMyState();
                                if (playerState == enemyState)
                                {
                                    counter = counter - 1;
                                    socket.Send(Encoding.ASCII.GetBytes(counter + " Counter " + "0 \r\n"));
                                }
                                Debug.WriteLine(counter + "counter");
                                if (counter == 0)
                                {
                                    MessageBox.Show("Meciul s-a terminat la egalitate!");
                                    if (gameState == GameState.AgainstPlayer)
                                    {
                                        socket.Send(Encoding.ASCII.GetBytes(0 + " Draw " + "0 \r\n"));
                                        socket.Shutdown(SocketShutdown.Both);
                                        socket.Close();
                                    }
                                    messageReceiver.WorkerSupportsCancellation = true;
                                    messageReceiver.CancelAsync();
                                    System.Windows.Forms.Application.Exit();
                                    return;
                                }
                                if (selectedCell.GetId() == board.GetCells().ElementAt(posibilities.ElementAt(posibilities.Count - 1).GetId()).GetId())
                                {
                                    MessageBox.Show("Tot tu esti.Fa o mutare!");
                                }
                                else
                                {
                                    VerifyIfDestroyedMill(selectedCell.GetId(), panelToVerify);
                                    if (gameState == GameState.AgainstPlayer)
                                    {
                                        socket.Send(Encoding.ASCII.GetBytes(selectedCell.GetId() + " Moving " + board.GetCells().ElementAt(posibilities.ElementAt(posibilities.Count - 1).GetId()).GetId() + " \r\n"));
                                    }
                                    VerifyCowInMill(panelToVerify);
                                    if (activePlayer.GetMyState() != Player.PlayerState.Taking)
                                    {
                                        GamePlay.SwitchTurn();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!GamePlay.CheckIfCanMove(GamePlay.GetInactivePlayer()))
                            {
                                MessageBox.Show("Ambii jucatori nu pot juca ");
                                if (gameState == GameState.AgainstPlayer)
                                {
                                    MessageBox.Show("Joc terminat.E egalitate!");
                                    if (gameState == GameState.AgainstPlayer)
                                    {
                                        socket.Send(Encoding.ASCII.GetBytes(0 + " Draw " + "0 \r\n"));
                                        socket.Shutdown(SocketShutdown.Both);
                                        socket.Close();
                                    }
                                    messageReceiver.WorkerSupportsCancellation = true;
                                    messageReceiver.CancelAsync();
                                    System.Windows.Forms.Application.Exit();
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Playerul nu se poate misca");
                                if (gameState == GameState.AgainstPlayer)
                                {
                                    MessageBox.Show("Joc terminat " + GamePlay.GetInactivePlayer().GetMyName() + " e castigator!");
                                    if (gameState == GameState.AgainstPlayer)
                                    {
                                        socket.Send(Encoding.ASCII.GetBytes(0 + " Victory " + GamePlay.GetInactivePlayer().GetMyName() + " \r\n"));
                                        socket.Shutdown(SocketShutdown.Both);
                                        socket.Close();
                                    }
                                    messageReceiver.WorkerSupportsCancellation = true;
                                    messageReceiver.CancelAsync();
                                    System.Windows.Forms.Application.Exit();
                                    return;
                                }
                            }
                        }

                    }
                }   

            }
        
        }

        private void Game_DragOver(object sender, DragEventArgs e)
        {
            Player activePlayer = GamePlay.GetActivePlayer();
            string currentPlayerName = labelName1.Text;
            Player.PlayerState playerState = activePlayer.GetMyState();
            if (activePlayer.GetMyName().Equals(currentPlayerName))
            {
                Point relativePoint = this.PointToClient(Cursor.Position);
                selectedCell = e.Data.GetData(e.Data.GetFormats()[0]) as BoardCell;
                bool color = GamePlay.GetInactivePlayer().GetMyColor();
                if (selectedCell != null && (selectedCell.GetState() != (color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied)))
                {
                        for (int i = 0; i < board.GetCells().Count; i++)
                        {
                            if (((board.GetCells()[i].GetX_Position() + 40) >= relativePoint.X) && ((board.GetCells()[i].GetX_Position() - 40) <= relativePoint.X) && ((board.GetCells()[i].GetY_Position() + 40) >= relativePoint.Y) && ((board.GetCells()[i].GetY_Position() - 40) <= relativePoint.Y) && (board.GetCells()[i].GetState() == BoardCell.CellState.Empty))
                            {
                                switch (playerState)
                                {
                                    case Player.PlayerState.Moving:
                                        if (selectedCell.GetNeighbors().Contains(board.GetCells()[i].GetId()) || selectedCell.GetId() == board.GetCells()[i].GetId())
                                        {
                                            if (!posibilities.Contains(board.GetCells()[i]))
                                            {
                                                posibilities.Add(board.GetCells()[i]);
                                            }
                                        }
                                        break;
                                    case Player.PlayerState.Flying:
                                        if (!posibilities.Contains(board.GetCells()[i]))
                                        {
                                            posibilities.Add(board.GetCells()[i]);
                                        }
                                        break;
                                }
                            }
                        }
                        if (posibilities.Count > 0)
                        {
                            board.GetCells().ElementAt(selectedCell.GetId()).SetState(BoardCell.CellState.Empty);
                            color = activePlayer.GetMyColor();
                            board.GetCells().ElementAt(posibilities.ElementAt(posibilities.Count - 1).GetId()).SetState(color == true ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
                            board.UpdateCells();
                            List<BoardCell> aux = new List<BoardCell>();
                            aux.Add(board.GetCells().ElementAt(posibilities.ElementAt(posibilities.Count - 1).GetId()));
                            for (int i = 0; i < posibilities.Count - 1; i++)
                            {
                                if (!activePlayer.GetMyBoardCells().Contains(board.GetCells().ElementAt(posibilities[i].GetId())))
                                {
                                    board.GetCells().ElementAt(posibilities[i].GetId()).SetState(BoardCell.CellState.Empty);
                                    board.UpdateCells();
                                }
                            }
                            posibilities.Clear();
                            posibilities = aux;
                        }
                    }
                }
                   
        }

        private void Game_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
    }
}
