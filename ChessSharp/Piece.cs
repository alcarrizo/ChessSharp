using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    public enum Type { PAWN, KNIGHT, QUEEN, KING, BISHOP, ROOK, EMPTY }
    abstract class Piece
    {
        public bool Color { get; set; }
        public string Name { get; set; }

        public Type Type { get; set; }

        public int Id { get; set; }

        
        abstract public bool ValidMove(int startX, int startY, int endX, int endY, Piece[,] Board);
        abstract public bool ValidPath(int startX, int startY, int endX, int endY, Piece[,] Board);

    }
}
