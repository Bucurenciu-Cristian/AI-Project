using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class Game
    {   enum GamePhases
        {
            placingPhase,
            middlePhase,
            flyingPhase,
            endGame
        }

        Board gameBoard;

        Game()
        {
            gameBoard = new Board();
        }

    }
}
