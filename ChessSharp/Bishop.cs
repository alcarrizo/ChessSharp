using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    class Bishop : Piece
    {


        public Bishop(bool c, int x)
        {
            Id = x;
            Color = c;
            if (Color)
                Name = "white_bishop";
            else
                Name = "black_bishop";
        }
        public override bool ValidMove(Point start, Point end, Piece[,] Board)
        {
            double slope = Math.Abs((double)end.Y - (double)start.Y) / Math.Abs((double)end.X - (double)start.X);

            if (slope == 1 && ValidPath(start, end, Board))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool ValidPath(Point start, Point end, Piece[,] Board)
        {
            //if(endY < startY || endX < startX)
            //{
            int changeX = 1;
            int changeY = 1;
            if (start.X > end.X)
            {
                changeX = -1;
            }
            if (start.Y > end.Y)
            {
                changeY = -1;
            }
            while (end.X != (start.X + changeX) && end.Y != (start.Y + changeY))
            {
                start.Y += changeY;
                start.X += changeX;
                if (Board[start.X, start.Y] != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
