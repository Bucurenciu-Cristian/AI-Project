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
        private static List<BoardCell> millCells = new List<BoardCell>();
        public GamePlay()
        {

        }
        public static void whoStartsTheGame()
        {
            Random random = new Random();
            myTurn1 = random.Next(1, 10);
            myTurn2 = random.Next(1, 10);
            if (myTurn1 == myTurn2)
                whoStartsTheGame();
        }
        public static void createPlayers()
        {
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
        public static Player getActivePlayer()
        {
            if (player1.GetMyTurn() == true)
                return player1;
            else
                return player2;
        }
        public static Player getInactivePlayer()
        {
            if (player1.GetMyTurn() == false)
                return player1;
            else
                return player2;
        }
        public static void playerTurn(Player play1, Player play2, int indice)
        {
            play1.GetMyHandCells().RemoveAt(indice-1);// posibil fara -1
            play1.SetMyTurn(false);
            play2.SetMyTurn(true);
        }
        public static void CheckForMill(Player activePlayer, BoardCell cell)
        {
            BoardCell currentCell = cell;
            List<BoardCell> allCells = Game.GetBoard().GetCells();
            Debug.WriteLine("celula " + currentCell.GetId() + " are " + currentCell.GetNeighbors().Length + " vecini:");
            foreach (int neighbor in currentCell.GetNeighbors())
            {
                Debug.WriteLine(neighbor);
            }
            for (int i = 0; i < currentCell.GetNeighbors().Length; i++)
            {
                // trei puncte sunt coliniare daca aria triunghiului format de cele 3 puncte = 0
                // poti face 2 mori deodata
                BoardCell checkedCell = allCells.ElementAt(currentCell.GetNeighbors()[i]-1);
                Debug.WriteLine(checkedCell.GetId()+" vecin");
                if (CheckPlayerHasCell(activePlayer, checkedCell.GetId()) && CheckNotSame(currentCell))
                {
                    Debug.WriteLine("Am adaugat celula " + currentCell.GetId());
                    millCells.Add(currentCell);
                    //sa fie diferit de ce am adaugat pana acum
                    if (millCells.Count > 2)
                    {
                        Debug.WriteLine("a intrat0000000000000000000000000");
                        //calculeaza arie
                        double Area = CalculateTriangleArea();
                        Debug.WriteLine("Aria: " + Area);
                        if (Area == 0.0)
                        //createMill pe locul gol
                        {
                            for(int j=0;j<activePlayer.GetMyMills().Length;j++)
                            {
                                if (activePlayer.GetMyMills()[j].GetIsNew()==false)
                                {
                                    activePlayer.GetMyMills()[j] = new Mill(millCells.ElementAt(0), millCells.ElementAt(1), millCells.ElementAt(2));
                                    Debug.WriteLine("a creat moara din " + millCells.ElementAt(0).GetId() +" "+ millCells.ElementAt(1).GetId() + " " + millCells.ElementAt(2).GetId());
                                    break;
                                }
                            }
                        }
                        //goleste lista
                        millCells.Clear();
                    }
                    else
                    {
                       CheckForMill(activePlayer, checkedCell);//merg mai departe in adancime
                    }                 
                }
            }
        }

        public static double CalculateTriangleArea()
        {
            // formula lui Heron cu semiperimetru
            // latura distanta dintre 2 puncte
            // cell(0) e vecin cu cell(1) si cell(1) vecin cu cell(2) 
            double[] sideLength = new double[3];
            double x1 = millCells.ElementAt(0).GetX_Position();
            double y1 = millCells.ElementAt(0).GetY_Position();
            double x2 = millCells.ElementAt(1).GetX_Position();
            double y2 = millCells.ElementAt(1).GetY_Position();
            double x3 = millCells.ElementAt(2).GetX_Position();
            double y3 = millCells.ElementAt(2).GetY_Position();
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
            if(mill !=null)
            {
                return CheckIsNew(mill);
            }
            return false;
        }
        public static bool CheckNotSame(BoardCell cell)
        {
            if(millCells.Contains(cell))
            {
                return false;
            }
            return true;
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
    }
}
