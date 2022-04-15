using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morabaraba
{
    internal class Piece
    {
        public PictureBox pieceImage;
        public Piece()
        {
            pieceImage = new PictureBox();
            pieceImage.Width = 25;
            pieceImage.Height = 25;
            pieceImage.Location = new Point(0, 0);
            pieceImage.BringToFront();
            pieceImage.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
