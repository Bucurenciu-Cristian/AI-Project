using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    public class Board
    {
        private List<BoardCell> cells;
        private Panel[] panels;
        private static PictureBox boardImage;
        public Board()
        {
            InitializeCells();
            InitializePanels();
            UpdateCells();
            boardImage = new PictureBox();
            boardImage.Image = Properties.Resources.game_board;

        }
        private void InitializeCells()
        {
            cells = new List<BoardCell>();
            
            cells.Add(new BoardCell(1, new int[] {2, 4, 10}));
            cells.ElementAt(0).SetX_Position(42);
            cells.ElementAt(0).SetY_Position(45);

            cells.Add(new BoardCell(2, new int[] {1, 3, 5}));
            cells.ElementAt(1).SetX_Position(480);
            cells.ElementAt(1).SetY_Position(45);
            
            cells.Add(new BoardCell(3, new int[] {2, 6, 15}));
            cells.ElementAt(2).SetX_Position(918);
            cells.ElementAt(2).SetY_Position(45);
            
            cells.Add(new BoardCell(4, new int[] {1, 5, 7, 11}));
            cells.ElementAt(3).SetX_Position(185);
            cells.ElementAt(3).SetY_Position(190);

            cells.Add(new BoardCell(5, new int[] {2, 4, 6, 8}));
            cells.ElementAt(4).SetX_Position(480);
            cells.ElementAt(4).SetY_Position(190);

            cells.Add(new BoardCell(6, new int[] {3, 5, 9, 14}));
            cells.ElementAt(5).SetX_Position(775);
            cells.ElementAt(5).SetY_Position(190);

            cells.Add(new BoardCell(7, new int[] {4, 8, 12}));
            cells.ElementAt(6).SetX_Position(335);
            cells.ElementAt(6).SetY_Position(335);

            cells.Add(new BoardCell(8, new int[] {5, 7, 9}));
            cells.ElementAt(7).SetX_Position(480);
            cells.ElementAt(7).SetY_Position(335);

            cells.Add(new BoardCell(9, new int[] {6, 8, 13}));
            cells.ElementAt(8).SetX_Position(625);
            cells.ElementAt(8).SetY_Position(335);

            cells.Add(new BoardCell(10, new int[] {1, 11, 22}));
            cells.ElementAt(9).SetX_Position(42);
            cells.ElementAt(9).SetY_Position(483);

            cells.Add(new BoardCell(11, new int[] {4, 10, 12, 19}));
            cells.ElementAt(10).SetX_Position(185);
            cells.ElementAt(10).SetY_Position(483);

            cells.Add(new BoardCell(12, new int[] {7, 11, 16}));
            cells.ElementAt(11).SetX_Position(335);
            cells.ElementAt(11).SetY_Position(483);

            cells.Add(new BoardCell(13, new int[] {9, 14, 18}));
            cells.ElementAt(12).SetX_Position(625);
            cells.ElementAt(12).SetY_Position(483);

            cells.Add(new BoardCell(14, new int[] {6, 13, 15, 21}));
            cells.ElementAt(13).SetX_Position(775);
            cells.ElementAt(13).SetY_Position(483);

            cells.Add(new BoardCell(15, new int[] {3, 14, 24}));
            cells.ElementAt(14).SetX_Position(918);
            cells.ElementAt(14).SetY_Position(483);

            cells.Add(new BoardCell(16, new int[] {12, 17, 19}));
            cells.ElementAt(15).SetX_Position(335);
            cells.ElementAt(15).SetY_Position(630);

            cells.Add(new BoardCell(17, new int[] {16, 18, 20}));
            cells.ElementAt(16).SetX_Position(480);
            cells.ElementAt(16).SetY_Position(630);

            cells.Add(new BoardCell(18, new int[] {13, 17, 21}));
            cells.ElementAt(17).SetX_Position(625);
            cells.ElementAt(17).SetY_Position(630);

            cells.Add(new BoardCell(19, new int[] {11, 16, 20, 22}));
            cells.ElementAt(18).SetX_Position(185);
            cells.ElementAt(18).SetY_Position(775);

            cells.Add(new BoardCell(20, new int[] {17, 19, 21, 23}));
            cells.ElementAt(19).SetX_Position(480);
            cells.ElementAt(19).SetY_Position(775);

            cells.Add(new BoardCell(21, new int[] {14, 18, 20, 24}));
            cells.ElementAt(20).SetX_Position(775);
            cells.ElementAt(20).SetY_Position(775);

            cells.Add(new BoardCell(22, new int[] {10, 19, 23}));
            cells.ElementAt(21).SetX_Position(42);
            cells.ElementAt(21).SetY_Position(925);

            cells.Add(new BoardCell(23, new int[] {20, 22, 24}));
            cells.ElementAt(22).SetX_Position(480);
            cells.ElementAt(22).SetY_Position(925);

            cells.Add(new BoardCell(24, new int[] {15, 21, 23}));
            cells.ElementAt(23).SetX_Position(918);
            cells.ElementAt(23).SetY_Position(925);
        }
        private void InitializePanels()
        {
            panels = new Panel[24];
            for(int i = 0; i < cells.Count; i++)
            {
                panels[i] = new Panel()
                {
                    Location = new System.Drawing.Point(cells.ElementAt(i).GetX_Position(), cells.ElementAt(i).GetY_Position()),
                    Width = 60,
                    Height = 60,
                    BackColor = Color.Transparent,
                    Name = cells.ElementAt(i).GetId()+""
                };
            }
        }

        public void UpdateCells()
        {
            for(int i = 0; i < cells.Count; i++)
            {
                //switch (i%3)
                //{
                //    case 0:
                //        panels[i].BackgroundImage = Properties.Resources.blackPiece;
                //        break;
                //    case 1:
                //        panels[i].BackgroundImage = Properties.Resources.whitePiece;
                //        break;
                //    case 2:
                //    default:
                //        panels[i].BackgroundImage = null;
                //        panels[i].BackColor = Color.Transparent;
                //        break;
                //}
                switch (cells.ElementAt(i).GetState())
                {
                    case BoardCell.CellState.BlackOccupied:
                        panels[i].BackgroundImage = Properties.Resources.blackPiece;
                        break;
                    case BoardCell.CellState.WhiteOccupied:
                        panels[i].BackgroundImage = Properties.Resources.whitePiece;
                        break;
                    case BoardCell.CellState.Empty:
                    default:
                        panels[i].BackgroundImage = null;
                        panels[i].BackColor = Color.Transparent;
                        break;
                }
            }
        }
        public List<BoardCell> GetCells()
        {
            return this.cells;
        }
        public Panel[] GetPanels()
        {
            return this.panels;
        }
        public static PictureBox GetPictureBox()
        {
            return boardImage;
        }
    }
}
