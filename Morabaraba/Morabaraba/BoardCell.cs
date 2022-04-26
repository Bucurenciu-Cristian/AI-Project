using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    internal class BoardCell
    {
        public int id { get; private set; }
        private int[] neighbors;
        public System.Drawing.Point Location;
        public CellState State { get; set; }
        private bool partOfThree { get; set; }

        public BoardCell(){}

        public BoardCell(int id, int[] neighbors)
        {
            this.id = id;
            this.neighbors = neighbors;
            State = CellState.Empty;
        }

        public void setX_Position(int xPosition)
        {
            this.Location.X = xPosition;
        }

        public int getX_Position()
        {
            return this.Location.X;
        }

        public void setY_Position(int yPosition)
        {
            this.Location.Y = yPosition;
        }

        public int getY_Position()
        {
            return this.Location.Y;
        }

        public int[] getNeighbors()
        {
            return this.neighbors;
        }


    }
}
