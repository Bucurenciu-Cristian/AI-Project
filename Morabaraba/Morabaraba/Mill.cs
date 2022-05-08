using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    public class Mill
    {
        //vecini id piese le tine deja minte
        //e noua deci poti lua o piesa a adversarului
        //culoare e nevoie?
        //la desfacere mill[i] = null;
        private bool isNew;
        private List<BoardCell> millCells;
        public Mill()
        {

        }
        public Mill( BoardCell cell1, BoardCell cell2, BoardCell cell3)
        {
            millCells = new List<BoardCell>();
            this.millCells.Add(cell1);
            this.millCells.Add(cell2);
            this.millCells.Add(cell3);
            this.isNew = true;//dupa luat piesa il punem pe fals
        }
        public bool GetIsNew()
        {
            return this.isNew;
        }
        public List<BoardCell> GetMillCells()
        {
            return this.millCells;
        }
        public void SetIsNew(bool isNew)
        {
            this.isNew = isNew;
        }
        public void SetMillCells(List<BoardCell> millCells)
        {
            this.millCells = millCells;
        }
        public void ResetMillCells()
        {
            for (int i = 0; i < millCells.Count; i++)
                millCells[i].ResetIsVisited();
        }
    }
}
