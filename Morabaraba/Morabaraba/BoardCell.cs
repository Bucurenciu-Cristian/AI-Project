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
        public enum CellPosition
        {
            Corner,
            Middle,
            Both
        }
        private CellPosition cellPosition;
        private CellState cellState;
        private bool partOfThree;// daca se afla in una din morile playerilor
        private bool isVisited;
        public BoardCell(){}

        public BoardCell(int id, int[] neighbors, CellPosition position)
        {
            this.id = id;
            this.neighbors = neighbors;
            cellPosition = position;
            cellState = CellState.Empty;
            partOfThree = false;
            isVisited = false;
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
            return this.cellState;
        }
        public void SetState(CellState state)
        {
            this.cellState = state;
        }
        public bool GetPartOfThree()
        {
            return this.partOfThree;
        }
        public void SetPartOfThree(bool isPart)
        {
            this.partOfThree = isPart;
        }
        public CellPosition GetCellPosition()
        {
            return cellPosition;
        }
        public void SetCellPosition(CellPosition position)
        {
            this.cellPosition = position;
        }
        public bool GetIsVisited()
        {
            return isVisited;
        }
        public void SetIsVisited()
        {
            this.isVisited = true;
        }
        public void ResetIsVisited()
        {
            this.isVisited = false;
        }
    }
}
