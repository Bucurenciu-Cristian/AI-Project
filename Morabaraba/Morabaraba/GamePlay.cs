using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class GamePlay
    {
        public static int myTurn1;
        public static int myTurn2;
        public static Player player1;
        public static Player player2;

        public GamePlay()
        {

        }

        public static void setMyTurn1(int myTurn)
        {
            myTurn1 = myTurn;
        }
        public static void setMyTurn2(int myTurn)
        {
            myTurn2 = myTurn;
        }
        public static void whoStartsTheGame()
        {
            Random random = new Random();
            myTurn1 = random.Next(1, 10);
            myTurn2 = random.Next(1, 10);
            if(myTurn1 == myTurn2)
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
            if (player1.myTurn == true)
                return player1;
            else
                return player2;
        }
        public static Player getInactivePlayer()
        {
            if (player1.myTurn == false)
                return player1;
            else
                return player2;
        }
        public static void playerTurn(Player play1, Player play2, int indice)
        {
            play1.myHandCells.RemoveAt(indice-1);// posibil fara -1
            play1.setMyTurn(false);
            play2.setMyTurn(true);
        }
    }
}
