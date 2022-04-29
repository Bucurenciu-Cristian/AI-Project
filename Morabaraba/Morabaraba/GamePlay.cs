using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class GamePlay
    {
        int myTurn1;
        int myTurn2;
        public void whoStartsTheGame()
        {
            Random random = new Random();
            myTurn1 = random.Next(1, 10);
            myTurn2 = random.Next(1, 10);
            if(myTurn1 == myTurn2)
                whoStartsTheGame();
        }
        public void createPlayers()
        {
            if(myTurn1 > myTurn2)
            {
                Player player1 = new Player(true, true);//e alb si incepe
                Player player2 = new Player(false, false);//e negru si asteapta randul
            }
            else
            {
                Player player1 = new Player(false, false);
                Player player2 = new Player(true, true);
            }
        }
    }
}
