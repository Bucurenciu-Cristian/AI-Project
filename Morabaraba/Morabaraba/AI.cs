using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    public class AI
    {
        Tuple<int, int> nextMaxMove;
        Tuple<int, int> nextMinMove;
        public const int Depth = 5;

        public void PlaceRandomPiece()
        {
            Random random = new Random();
            int cellId = random.Next(1,24);
            Game.GetBoard().GetCells().ElementAt(cellId).SetState(GamePlay.GetPlayer2().GetMyColor() ? BoardCell.CellState.WhiteOccupied : BoardCell.CellState.BlackOccupied);
            GamePlay.GetActivePlayer().GetMyBoardCells().Add(Game.GetBoard().GetCells().ElementAt(cellId));
            Game.GetBoard().UpdateCells();
        }

        public int PiecesCountGameEvaluation(Player player1, Player player2)
        {
            if (player2.GetMyName() == "PC" && player2.GetMyColor())
            {
                if (player1.GetMyColor())
                {
                    return player1.GetMyBoardCells().Count - player2.GetMyBoardCells().Count;
                }
                else
                {
                    return player2.GetMyBoardCells().Count - player1.GetMyBoardCells().Count;
                }
            }
            else
            {
                if (player1.GetMyColor())
                {
                    return player2.GetMyBoardCells().Count - player1.GetMyBoardCells().Count;
                }
                else
                {
                    return player1.GetMyBoardCells().Count - player2.GetMyBoardCells().Count;
                }
            }

        }

        public int MillsCountGameEvaluation(Player player1, Player player2)
        {
            if (player2.GetMyName() == "PC" && player2.GetMyColor())
            {
                if (player1.GetMyColor())
                {
                    return player1.GetMyMills().Length * 10;
                }
                else
                {
                    return player2.GetMyMills().Length * (-10);
                }
            }
            else
            {
                if (player1.GetMyColor())
                {
                    return player2.GetMyMills().Length * (-10);
                }
                else
                {
                    return player1.GetMyMills().Length * 10;
                }
            }

        }

        public int[] GetOccupiedPositions(int cellId,BoardCell.CellState cellState, List<BoardCell> boardCells)
        {
            int[] occupiedPositions = new int[24];
            int index = 0;
            foreach(BoardCell cell in boardCells)
            {
                if(cell.GetState() == cellState)
                {
                    occupiedPositions[index] = cell.GetId();
                }
                else
                {
                    occupiedPositions[index] = -1;
                }
                index++;
            }
            return occupiedPositions;
        }

        public Tuple<int,int>[] AlmostMill(bool color, List<BoardCell> boardCells)
        {
            int[] occupiedPositions = new int[4];
            Tuple<int, int>[] almostMill = new Tuple<int, int>[100];
            int aux = 0;

            if (!color)//white
            {
                foreach(BoardCell cell in boardCells)
                {
                    occupiedPositions = new int[4];
                    occupiedPositions = GetOccupiedPositions(cell.GetId(), BoardCell.CellState.BlackOccupied, boardCells);
                    for (int i = 0; i < 4; i++)
                    {
                        if(occupiedPositions[i] != -1)
                        {
                            almostMill[aux] = new Tuple<int, int>(cell.GetId(),occupiedPositions[i]);
                            aux++;
                        }
                    }
                }
            }
            else//black
            {
                foreach (BoardCell cell in boardCells)
                {
                    occupiedPositions = new int[4];
                    occupiedPositions = GetOccupiedPositions(cell.GetId(), BoardCell.CellState.BlackOccupied, boardCells);
                    for (int i = 0; i < 4; i++)
                    {
                        if (occupiedPositions[i] != -1)
                        {
                            almostMill[aux] = new Tuple<int, int>(cell.GetId(), occupiedPositions[i]);
                            aux++;
                        }
                    }
                }
            }
            return almostMill;
        }
    
        public int ChooseWhichCow(bool color, List<BoardCell> boardCells)
        {
            Tuple<int,int>[] almostMill = AlmostMill(color, boardCells);
            int almostMillCounter = 0;
            int index;

            for(int i = 0; i < 100; i++)
            {
                if(almostMill[i] == null)
                {
                    break;
                }
                else
                {
                    almostMillCounter++;
                }
            }
            Random rand = new Random();
            if(almostMillCounter > 0)
            {
                int number = rand.Next(0, almostMillCounter - 1);
                index = rand.Next(0, 1);
                if (index == 0)
                {
                    return almostMill[number].Item1;
                }
                else
                {
                    return almostMill[number].Item2;
                }
            }
            else
            {
                while (true)
                {
                    index = (short)rand.Next(0, 23);
                    if (color)
                    {
                        if(boardCells.ElementAt(index).GetState() == BoardCell.CellState.BlackOccupied)
                        {
                            return boardCells.ElementAt((int)index).GetId();
                        }
                    }
                    else
                    {
                        if (boardCells.ElementAt(index).GetState() == BoardCell.CellState.WhiteOccupied)
                        {
                            return boardCells.ElementAt((int)index).GetId();
                        }
                    }
                }
            }
        }

        public Tuple<int, int> MoveMiniMax(bool color, List<BoardCell> boardCells)
        {
            int bestMoveValue = MiniMax(color, boardCells, Depth,boardCells);
            Tuple<int, int> bestMove;
            if (color)
            {
                bestMove = nextMaxMove;
            }
            else
            {
                bestMove = nextMinMove;
            }
            return bestMove;
        }

        public int MiniMax(bool color, List<BoardCell> originalBoard, int depth, List<BoardCell> cloneBoard)
        {
            if(depth == 0)
            {
                return PiecesCountGameEvaluation(GamePlay.GetPlayer1(), GamePlay.GetPlayer2());
            }
            if (color)
            {
                //white
                int maxEval = Int32.MinValue;
                foreach(Tuple<int,int> move in GetAllPossibleMoves(color, cloneBoard))
                {
                    if((move != null) && (move.Item1 != -1 && move.Item2 != -1))
                    {
                        List<BoardCell> copyBoard = new List<BoardCell>(cloneBoard);
                        if ((GamePlay.GetPlayer1().GetMyState() == Player.PlayerState.Placing || GamePlay.GetPlayer2().GetMyState() == Player.PlayerState.Placing))
                        {
                            copyBoard.ElementAt(move.Item1).SetState(BoardCell.CellState.WhiteOccupied);
                        }
                        else
                        {
                            copyBoard.ElementAt(move.Item1).SetState(BoardCell.CellState.Empty);
                            copyBoard.ElementAt(move.Item2).SetState(BoardCell.CellState.WhiteOccupied);
                        }
                        Player activePlayer = GamePlay.GetPlayer1().GetMyColor()?GamePlay.GetPlayer1():GamePlay.GetPlayer2();
                        //GamePlay.CheckForMill(activePlayer, copyBoard.ElementAt(move.Item2));
                        for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
                        {
                            if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                            {
                                copyBoard = TakeCow(color, copyBoard);
                            }
                        }
                        int eval = MiniMax(!color, copyBoard,depth - 1, copyBoard);
                        if(depth == Depth && eval > maxEval)
                        {
                            nextMaxMove = move;
                        }
                        maxEval = Math.Max(maxEval, eval);
                    }
                }
                return maxEval;
            }
            else
            {
                //black
                int minEval = Int32.MinValue;
                foreach (Tuple<int, int> move in GetAllPossibleMoves(color, cloneBoard))
                {
                    if ((move != null) && (move.Item1 != -1 && move.Item2 != -1))
                    {
                        List<BoardCell> copyBoard = new List<BoardCell>(cloneBoard);
                        if ((GamePlay.GetPlayer1().GetMyState() == Player.PlayerState.Placing || GamePlay.GetPlayer2().GetMyState() == Player.PlayerState.Placing))
                        {
                            copyBoard.ElementAt(move.Item1).SetState(BoardCell.CellState.BlackOccupied);
                        }
                        else
                        {
                            copyBoard.ElementAt(move.Item1).SetState(BoardCell.CellState.Empty);
                            copyBoard.ElementAt(move.Item2).SetState(BoardCell.CellState.BlackOccupied);
                        }
                        Player activePlayer = GamePlay.GetPlayer1().GetMyColor() ? GamePlay.GetPlayer1() : GamePlay.GetPlayer2();
                        //GamePlay.CheckForMill(activePlayer, copyBoard.ElementAt(move.Item2));
                        for (int i = 0; i < activePlayer.GetMyMills().Count(); i++)
                        {
                            if (GamePlay.CheckMillIsNew(activePlayer.GetMyMills()[i]))
                            {
                                copyBoard = TakeCow(color, copyBoard);
                            }
                        }
                        int eval = MiniMax(!color, originalBoard, depth - 1, copyBoard);
                        if (depth == Depth && eval < minEval)
                        {
                            nextMinMove = move;
                        }
                        minEval = Math.Max(minEval, eval);
                    }
                }
                return minEval;
            }
        }

        public List<BoardCell> TakeCow(bool color, List<BoardCell> currentBoard)
        {
            int id = ChooseWhichCow(color, currentBoard);
            currentBoard.ElementAt(id).SetState(BoardCell.CellState.Empty);
            Player player;
            if(GamePlay.GetPlayer1().GetMyColor() != color)
            {
                player = GamePlay.GetPlayer1();
            }
            else
            {
                player = GamePlay.GetPlayer2();
            }
            player.GetMyBoardCells().Remove(currentBoard.ElementAt(id));

            return currentBoard;
        }

        public int[] GetFreeNeighboorPositions(int cellId,List<BoardCell> boardCells)
        {
            int[] freeNeighboorPositions = new int[4];
            int[] neighboors = boardCells.ElementAt(cellId).GetNeighbors();

            for(int i =0; i < 4; i++)
            {
                if (neighboors[i] != -1 && boardCells.ElementAt(neighboors[i]).GetState() == BoardCell.CellState.Empty)
                {
                    freeNeighboorPositions[i] = boardCells.ElementAt(neighboors[i]).GetId();
                }
                else
                {
                    freeNeighboorPositions[i] = -1;
                }
            }
            return freeNeighboorPositions;
        }

        public Tuple<int,int>[] GetAllPossibleMoves(bool color, List<BoardCell> boardCells)
        {
            Tuple<int, int>[] possibleMoves = new Tuple<int, int>[100];

            if(GamePlay.GetPlayer1().GetMyState() == Player.PlayerState.Placing || GamePlay.GetPlayer2().GetMyState() == Player.PlayerState.Placing
                || GamePlay.GetPlayer1().GetMyState() == Player.PlayerState.Flying || GamePlay.GetPlayer2().GetMyState() == Player.PlayerState.Flying)
            {
                for(int i = 0; i < boardCells.Count; i++)
                {
                    if (boardCells.ElementAt(i).GetState() == BoardCell.CellState.Empty)
                    {
                        possibleMoves[i] = new Tuple<int, int>(boardCells.ElementAt(i).GetId(), boardCells.ElementAt(i).GetId());
                    }
                }
            }
            else
            {

                if (color)
                {
                    for(int i = 0; i < boardCells.Count; i++)
                    {
                        if(boardCells.ElementAt(i).GetState() == BoardCell.CellState.WhiteOccupied)
                        {
                            int[] freeNeighboors = GetFreeNeighboorPositions(boardCells.ElementAt(i).GetId(), boardCells);
                            for(int j = 0; j < 4; j++)
                            {
                                possibleMoves[i * 4 + j] = new Tuple<int, int>(boardCells.ElementAt(i).GetId(),freeNeighboors[j]);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < boardCells.Count; i++)
                    {
                        if (boardCells.ElementAt(i).GetState() == BoardCell.CellState.BlackOccupied)
                        {
                            int[] freeNeighboors = GetFreeNeighboorPositions(boardCells.ElementAt(i).GetId(), boardCells);
                            for (int j = 0; j < 4; j++)
                            {
                                possibleMoves[i * 4 + j] = new Tuple<int, int>(boardCells.ElementAt(i).GetId(), freeNeighboors[j]);
                            }
                        }
                    }
                }
            }
            return possibleMoves;
        }
    }
}
