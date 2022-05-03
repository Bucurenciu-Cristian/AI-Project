using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    public class BoardCell
    {
        private int id;
        private int[] neighbors;
        private System.Drawing.Point Location;
        public enum CellState
        {
            Empty,
            BlackOccupied,
            WhiteOccupied
        }
        private CellState state;
        private bool partOfThree;// daca se afla in una din morile playerilor

        public BoardCell(){}

        public BoardCell(int id, int[] neighbors)
        {
            this.id = id;
            this.neighbors = neighbors;
            state = CellState.Empty;
        }

        public void SetX_Position(int xPosition)
        {
            this.Location.X = xPosition;
        }

        public int GetX_Position()
        {
            return this.Location.X;
        }

        public void SetY_Position(int yPosition)
        {
            this.Location.Y = yPosition;
        }

        public int GetY_Position()
        {
            return this.Location.Y;
        }

        public int[] GetNeighbors()
        {
            return this.neighbors;
        }
        public int GetId()
        {
            return this.id;
        }
        public void SetId(int id)
        {
            this.id = id;
        }
        public CellState GetState()
        {
            return this.state;
        }
        public void SetState(CellState state)
        {
            this.state = state;
        }
        public bool GetPartOfThree()
        {
            return this.partOfThree;
        }
        public void SetPartOfThree(bool isPart)
        {
            this.partOfThree = isPart;
        }
    }
}
