using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class BoardCell
    {
        private int id;
        private int[] neighbors;

        private int xPosition;
        private int yPosition;

        public BoardCell(){}

        public BoardCell(int id, int[] neighbors)
        {
            this.id = id;
            this.neighbors = neighbors;
        }

        public void setX_Position(int xPosition)
        {
            this.xPosition = xPosition;
        }

        public int getX_Position()
        {
            return this.xPosition;
        }

        public void setY_Position(int yPosition)
        {
            this.yPosition = yPosition;
        }

        public int getY_Position()
        {
            return this.yPosition;
        }
    }
}
