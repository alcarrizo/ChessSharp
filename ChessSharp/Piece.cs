using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    public enum Type { PAWN, KNIGHT, QUEEN, KING, BISHOP, ROOK, EMPTY }
    abstract class Piece
    {
        public bool Color { get; set; }
        public string Name { get; set; }

        public int Id { get; set; }

        //public Image Image{get; set;}


        abstract public bool ValidMove(Point start, Point end, Piece[,] Board);
        abstract public bool ValidPath(Point start, Point end, Piece[,] Board);

    }
}
