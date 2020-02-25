using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Knight : Piece
    {

        public Knight(bool c, int x)
        {
            Id = x;
            Type = Type.KNIGHT;
            Color = c;
            if (Color)
                Name = "white_knight";
            else
                Name = "black_knight";
        }
        public override bool ValidMove(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);

            if (slope == 0.5 || slope == 2)
            {
                if (Math.Abs((double)endY - (double)startY) <= 2 && Math.Abs((double)endX - (double)startX) <= 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override bool ValidPath(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            return Board[startX, startX].Color != Board[endX, endX].Color;
        }
    }
}
