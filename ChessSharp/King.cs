using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class King : Piece
    {


        public King(bool c, int x)
        {
            Id = x;
            Type = Type.KING;
            Color = c;
            if (Color)
                Name = "white_king";
            else
                Name = "black_king";
        }

        public override bool ValidMove(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            if (Math.Abs((double)endY - (double)startY) <= 1 && Math.Abs((double)endX - (double)startX) <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool ValidPath(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            //return KingInCheck(endX, endY, Board) == false;
            return true;
        }





    }
}
