using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    class Knight : Piece
    {
        public Knight(bool c, int x)
        {
            Id = x;
            Color = c;
            if (Color)
                Name = "white_knight";
            else
                Name = "black_knight";
        }
        public override bool ValidMove(Point start, Point end, Piece[,] Board)
        {
            double slope = (Math.Abs((double)end.Y - (double)start.Y) / Math.Abs((double)end.X - (double)start.X));

            if (slope == 0.5 || slope == 2)
            {
                if (Math.Abs((double)end.Y - (double)start.Y) <= 2 && Math.Abs((double)end.X - (double)start.X) <= 2)
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

        public override bool ValidPath(Point start, Point end, Piece[,] Board)
        {
            return Board[start.X, start.Y].Color != Board[end.X, end.Y].Color;
        }
    }
}
