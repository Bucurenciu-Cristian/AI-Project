using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    public class GamePlay
    {
        private static int myTurn1;
        private static int myTurn2;
        private static Player player1;
        private static Player player2;
        private static Mill mill;
        public GamePlay()
        {

        }
        public static void WhoStartsTheGame()
        {
            Random random = new Random();
            myTurn1 = random.Next(1, 10);
            myTurn2 = random.Next(1, 10);
            if (myTurn1 == myTurn2)
                WhoStartsTheGame();
        }
        public static void CreatePlayers()
        {
            mill = new Mill();
            mill.SetMillCells(new List<BoardCell>());
            if(myTurn1 > myTurn2)
            {
                player1 = new Player(true, true,"player1");//e alb si incepe
                player2 = new Player(false, false, "player2");//e negru si asteapta randul
            }
            else
            {
                player1 = new Player(false, false, "player1");
                player2 = new Player(true, true, "player2");
            }
        }
        public static Player GetActivePlayer()
        {
            if (player1.GetMyTurn() == true)
                return player1;
            else
                return player2;
        }
        public static Player GetInactivePlayer()
        {
            if (player1.GetMyTurn() == false)
                return player1;
            else
                return player2;
        }
        public static void PlayerTurn(Player play1, Player play2, int indice)
        {
            if(indice>0)
            {
                play1.GetMyHandCells().RemoveAt(indice - 1);
            }
            play1.SetMyTurn(false);
            play2.SetMyTurn(true);
        }
        public static void CheckForMillCorner(Player activePlayer, BoardCell cell)
        {
            // trei puncte sunt coliniare daca aria triunghiului format de cele 3 puncte = 0
            // poti face 2/3 mori deodata
            Debug.WriteLine("CheckForMillCorner");
            BoardCell currentCell = cell;
            List<BoardCell> allCells = Game.GetBoard().GetCells();
            Debug.WriteLine("celula " + currentCell.GetId() + " are " + currentCell.GetNeighbors().Length + " vecini:");
            foreach (int neighbor in currentCell.GetNeighbors())
            {
                Debug.Write(neighbor+ " ");
            }
            Debug.WriteLine("");
            mill.GetMillCells().Add(currentCell);
            Debug.WriteLine("Am adaugat " + currentCell.GetId());
            currentCell.SetIsVisited();
            for (int i = 0; i < mill.GetMillCells().Count; i++)
                Debug.WriteLine(mill.GetMillCells()[i].GetId() + " ");
            if (mill.GetMillCells().Count > 2)
            {
                double Area = CalculateTriangleArea();
                Debug.WriteLine("Aria: " + Area);
                if (Area == 0.0)
                {
                    for (int j = 0; j < activePlayer.GetMyMills().Length; j++)
                    {
                        if (activePlayer.GetMyMills()[j].GetIsNew() == false)
                        {
                            activePlayer.GetMyMills()[j] = new Mill(mill.GetMillCells().ElementAt(0), mill.GetMillCells().ElementAt(1), mill.GetMillCells().ElementAt(2));
                            SendMillMess(mill,"true");
                            Debug.WriteLine("A creat moara din " + mill.GetMillCells().ElementAt(0).GetId() + " " + mill.GetMillCells().ElementAt(1).GetId() + " " + mill.GetMillCells().ElementAt(2).GetId());
                            break;
                        }
                    }
                } 
                Debug.WriteLine("am scos pe " + mill.GetMillCells().ElementAt(2).GetId());
                mill.GetMillCells()[2].ResetIsVisited();
                mill.GetMillCells().RemoveAt(2);
                return;
            }
            else
            {
                for (int i = 0; i < currentCell.GetNeighbors().Length; i++)
                {
                    BoardCell checkedCell = allCells.ElementAt(currentCell.GetNeighbors()[i]);
                    if (CheckPlayerHasCell(activePlayer, checkedCell.GetId()) && (checkedCell.GetIsVisited() == false))
                    {
                        CheckForMillCorner(activePlayer, checkedCell);//merg mai departe in adancime
                    }
                }
                if (mill.GetMillCells().Count > 1 )// am ajuns la sfarsitul vecinilor si nu am gasit urmatoarea piesa
                {
                    Debug.WriteLine("am scos pe " + mill.GetMillCells().ElementAt(1).GetId());
                    mill.GetMillCells()[1].ResetIsVisited();
                    mill.GetMillCells().RemoveAt(1);
                    return;
                }
            }
        }
        public static void CheckForMillMiddle(Player activePlayer, BoardCell cell)
        {
            Debug.WriteLine("CheckForMillMiddle");
            BoardCell currentCell = cell;
            List<BoardCell> allCells = Game.GetBoard().GetCells();
            for (int i = 0; i < currentCell.GetNeighbors().Length; i++)
            {
                if (CheckPlayerHasCell(activePlayer, currentCell.GetNeighbors()[i]))
                {
                    mill.GetMillCells().Add(currentCell);
                    mill.GetMillCells().Add(allCells.ElementAt(currentCell.GetNeighbors()[i]));
                    for (int j = 0; j < currentCell.GetNeighbors().Length && j!=i; j++)
                    {
                        Debug.WriteLine(mill.GetMillCells().Contains(allCells.ElementAt(currentCell.GetNeighbors()[j])));
                        bool contains = mill.GetMillCells().Contains(allCells.ElementAt(currentCell.GetNeighbors()[j]));
                        Debug.WriteLine(currentCell.GetNeighbors()[j]);
                        if ((CheckPlayerHasCell(activePlayer, currentCell.GetNeighbors()[j])==true) && contains == false)
                        {
                            mill.GetMillCells().Add(allCells.ElementAt(currentCell.GetNeighbors()[j]) );
                            double Area = CalculateTriangleArea();
                            Debug.WriteLine("Aria: " + Area);
                            if (Area == 0.0)
                            {
                                for (int k = 0; k < activePlayer.GetMyMills().Length; k++)
                                {
                                    if (activePlayer.GetMyMills()[k].GetIsNew() == false)
                                    {
                                        activePlayer.GetMyMills()[k] = new Mill(mill.GetMillCells().ElementAt(0), mill.GetMillCells().ElementAt(1), mill.GetMillCells().ElementAt(2));
                                        Debug.WriteLine("A creat moara din " + mill.GetMillCells().ElementAt(0).GetId() + " " + mill.GetMillCells().ElementAt(1).GetId() + " " + mill.GetMillCells().ElementAt(2).GetId());
                                        SendMillMess(mill,"true");
                                        break;
                                    }
                                }
                            }
                            mill.GetMillCells().RemoveAt(2);
                        }
                    }
                    mill.GetMillCells().Clear();
                }
            }
        }
        public static void CheckForMill(Player activePlayer, BoardCell cell)
        {
            if (activePlayer.GetMyBoardCells().Count() > 2)
            {
                BoardCell.CellPosition position = cell.GetCellPosition();
                switch (position)
                {
                    case BoardCell.CellPosition.Corner:
                    {
                        Debug.WriteLine("----------------------------");
                        CheckForMillCorner(activePlayer, cell);
                        mill.GetMillCells()[0].ResetIsVisited();
                        mill.GetMillCells().RemoveAt(0);
                        break;
                    }
                    case BoardCell.CellPosition.Middle:
                    {
                        Debug.WriteLine("******************************");
                        CheckForMillMiddle(activePlayer, cell);
                        break;
                    }
                    case BoardCell.CellPosition.Both:
                    {
                        Debug.WriteLine("*******************************");
                        CheckForMillMiddle(activePlayer, cell);
                        Debug.WriteLine("----------------------------");
                        CheckForMillCorner(activePlayer, cell);
                        mill.GetMillCells()[0].ResetIsVisited();
                        mill.GetMillCells().RemoveAt(0);
                        break;
                    }
                }
            }
        }

        public static double CalculateTriangleArea()
        { 
            double[] sideLength = new double[3];//calcul laturi pentru heron
            double x1 = mill.GetMillCells().ElementAt(0).GetX_Position();
            double y1 = mill.GetMillCells().ElementAt(0).GetY_Position();
            double x2 = mill.GetMillCells().ElementAt(1).GetX_Position();
            double y2 = mill.GetMillCells().ElementAt(1).GetY_Position();
            double x3 = mill.GetMillCells().ElementAt(2).GetX_Position();
            double y3 = mill.GetMillCells().ElementAt(2).GetY_Position();
            sideLength[0] = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));//a
            sideLength[1] = Math.Sqrt(Math.Pow((x3 - x2), 2) + Math.Pow((y3 - y2), 2));//b
            sideLength[2] = Math.Sqrt(Math.Pow((x3 - x1), 2) + Math.Pow((y3 - y1), 2));//c
            double s = (sideLength[0] + sideLength[1] + sideLength[2]) / 2.0;
            double area = Math.Sqrt(s*(s-sideLength[0])*(s-sideLength[1])*(s-sideLength[2]));
            return area;
        }

        public static bool CheckPlayerHasCell(Player activePlayer, int cellId)
        {
            for(int i=0;i<activePlayer.GetMyBoardCells().Count;i++)
            {
                if(activePlayer.GetMyBoardCells()[i].GetId() == cellId)
                    return true;
            }
            return false;
        }

        public static bool CheckIsNew(Mill mill)
        {
            return mill.GetIsNew();
        }
        public static bool CheckMillIsNew( Mill mill)
        {
            if(mill !=new Mill())
            {
                return CheckIsNew(mill);
            }
            return false;
        }
        public static int NewPlayerMill(Player activePlayer)
        {
            int ct = 0;
            for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
            {
                if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                {
                    ct++;
                }
            }
            return ct;
        }
        public static bool AllPlayerCellsInMills()
        {
            Player inactivePlayer = GetInactivePlayer();
            int ctp = 0;
            for(int i = 0; i < inactivePlayer.GetMyBoardCells().Count;i++)
            {
                if(inactivePlayer.GetMyBoardCells()[i].GetPartOfThree())
                {
                    ctp++;
                }
            }
            if(ctp == inactivePlayer.GetMyBoardCells().Count)
                return true;
            return false;
        }
        public static void DestroyMill(BoardCell cell)
        {
            Player inactivePlayer = GetInactivePlayer();   
            for(int i=0;i < inactivePlayer.GetMyMills().Length;i++)
            {
                if (inactivePlayer.GetMyMills()[i].GetMillCells() != null)
                {
                    if (inactivePlayer.GetMyMills()[i].GetMillCells().Contains(cell))
                    {
                        inactivePlayer.GetMyMills()[i].ResetMillCells();
                        Game.GetSocket().Send(Encoding.ASCII.GetBytes(inactivePlayer.GetMyMills()[i].GetMillCells().ElementAt(0).GetId().ToString() + " PartOfThree" + " true" + " \r\n"));
                        inactivePlayer.GetMyMills()[i] = new Mill();
                    }
                }
            }
        }
        public static void SendMillMess( Mill mill, string partOfThree)
        {
            Game.GetSocket().Send(Encoding.ASCII.GetBytes(mill.GetMillCells().ElementAt(0).GetId().ToString() + " PartOfThree" + " " + partOfThree + " \r\n"));
            Game.GetSocket().Send(Encoding.ASCII.GetBytes(mill.GetMillCells().ElementAt(1).GetId().ToString() + " PartOfThree" + " " + partOfThree + " \r\n"));
            Game.GetSocket().Send(Encoding.ASCII.GetBytes(mill.GetMillCells().ElementAt(2).GetId().ToString() + " PartOfThree" + " " + partOfThree + " \r\n"));
            Game.GetBoard().GetCells().ElementAt(mill.GetMillCells().ElementAt(0).GetId()).SetPartOfThree(Convert.ToBoolean(partOfThree));
            Game.GetBoard().GetCells().ElementAt(mill.GetMillCells().ElementAt(1).GetId()).SetPartOfThree(Convert.ToBoolean(partOfThree));
            Game.GetBoard().GetCells().ElementAt(mill.GetMillCells().ElementAt(2).GetId()).SetPartOfThree(Convert.ToBoolean(partOfThree));
        }
        public static void SetMyTurn1(int myTurn)
        {
            myTurn1 = myTurn;
        }
        public static void SetMyTurn2(int myTurn)
        {
            myTurn2 = myTurn;
        }
        public static int GetMyTurn1()
        {
            return myTurn1;
        }
        public static int GetMyTurn2()
        {
            return myTurn2;
        }
        public static Player GetPlayer1()
        {
            return player1;
        }
        public static Player GetPlayer2()
        {
            return player2;
        }
        public static void SetPlayer1(Player player)
        {
            player1 = player;
        }
        public static void SetPlayer2(Player player)
        {
            player2 = player;
        }
        public static Mill GetMill()
        {
            return mill;
        }
        public static void SetMill(Mill newMill)
        {
            mill = newMill;
        }
    }
}
