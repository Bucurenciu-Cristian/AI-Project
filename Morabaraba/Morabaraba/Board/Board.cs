using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class Board
    {
        private List<BoardCell> cells;
        Piece piece = new Piece();

        public Board()
        {
            initializeBoard();
            base.Controls.Add(piece.pieceImage);
        }

        private void initializeBoard()
        {
            cells = new List<BoardCell>();
            
            cells.Add(new BoardCell(1, new int[] {2, 4, 10}));
            cells.Add(new BoardCell(2, new int[] {1, 3, 5}));
            cells.Add(new BoardCell(3, new int[] {2, 6, 15}));
            cells.Add(new BoardCell(4, new int[] {1, 5, 7, 11}));
            cells.Add(new BoardCell(5, new int[] {2, 4, 6, 8}));
            cells.Add(new BoardCell(6, new int[] {3, 5, 9, 14}));
            cells.Add(new BoardCell(7, new int[] {4, 8, 12}));
            cells.Add(new BoardCell(8, new int[] {5, 7, 9}));
            cells.Add(new BoardCell(9, new int[] {6, 8, 13}));
            cells.Add(new BoardCell(10, new int[] {1, 11, 22}));
            cells.Add(new BoardCell(11, new int[] {4, 10, 12, 19}));
            cells.Add(new BoardCell(12, new int[] {7, 11, 16}));
            cells.Add(new BoardCell(13, new int[] {9, 14, 18}));
            cells.Add(new BoardCell(14, new int[] {6, 13, 15, 21}));
            cells.Add(new BoardCell(15, new int[] {3, 14, 24}));
            cells.Add(new BoardCell(16, new int[] {12, 17, 19}));
            cells.Add(new BoardCell(17, new int[] {16, 18, 20}));
            cells.Add(new BoardCell(18, new int[] {13, 17, 21}));
            cells.Add(new BoardCell(19, new int[] {11, 16, 20, 22}));
            cells.Add(new BoardCell(20, new int[] {17, 19, 21, 23}));
            cells.Add(new BoardCell(21, new int[] {14, 18, 20, 24}));
            cells.Add(new BoardCell(22, new int[] {10, 19, 23}));
            cells.Add(new BoardCell(23, new int[] {20, 22, 24}));
            cells.Add(new BoardCell(24, new int[] {15, 21, 23}));
        
            for(int i=0; i<24; i++)
            {
                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                        
                        cells.ElementAt(i - 1).setX_Position(i * 150 + 50);
                        cells.ElementAt(i - 1).setY_Position(50);
                        break;
                    case 3:
                    case 4:
                    case 5:
                        cells.ElementAt(i - 1).setX_Position((i % 3 + 1) * 100);
                        cells.ElementAt(i - 1).setY_Position(100);
                        break;
                    case 6:
                    case 7:
                    case 8:
                        cells.ElementAt(i - 1).setX_Position((i % 3 + 3) * 50);
                        cells.ElementAt(i - 1).setY_Position(150);
                        break;
                    case 9:
                    case 10:
                    case 11:
                        cells.ElementAt(i - 1).setX_Position((i % 3) * 50);
                        cells.ElementAt(i - 1).setY_Position(200);
                        break;
                    case 12:
                    case 13:
                    case 14:
                        cells.ElementAt(i - 1).setX_Position((i % 3) * 50 + 200);
                        cells.ElementAt(i - 1).setY_Position(250);
                        break;
                    case 15:
                    case 16:
                    case 17:
                        cells.ElementAt(i - 1).setX_Position((i % 3 + 3) * 50);
                        cells.ElementAt(i - 1).setY_Position(300);
                        break;
                    case 18:
                    case 19:
                    case 20:
                        cells.ElementAt(i - 1).setX_Position((i % 3 + 1) * 100);
                        cells.ElementAt(i - 1).setY_Position(350);
                        break;
                    case 21:
                    case 22:
                    case 23:
                        cells.ElementAt(i - 1).setX_Position(i % 21 * 150 + 50);
                        cells.ElementAt(i - 1).setY_Position(400);
                        break;
                }
            }
        }
    }
}
