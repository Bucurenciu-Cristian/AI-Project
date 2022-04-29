using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class Mill
    {
        //vecini id piese le tine deja minte
        //e noua deci poti lua o piesa a adversarului
        //culoare e nevoie?
        //la desfacere mill[i] = null;
        bool isNew;
        List<BoardCell> millCells = new List<BoardCell>();
        public Mill()
        {

        }
        public Mill(bool color, BoardCell cell1, BoardCell cell2, BoardCell cell3)
        {
            this.millCells.Add(cell1);
            this.millCells.Add(cell2);
            this.millCells.Add(cell3);
            this.isNew = true;//dupa luat piesa il punem pe fals
        }
    }
}
