using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Bishop : Piece
    {


        public Bishop(bool c, int x)
        {
            Id = x;
            Type = Type.BISHOP;
            Color = c;
            if (Color)
                Name = "white_bishop";
            else
                Name = "black_bishop";
        }
        public override bool ValidMove(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);

            if (slope == 1 && ValidPath(startX, startY, endX, endY, Board))
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
            //if(endY < startY || endX < startX)
            //{
            int changeX = 1;
            int changeY = 1;
            if (startX > endX)
            {
                changeX = -1;
            }
            if (startY > endY)
            {
                changeY = -1;
            }
            while (endX != (startX + changeX) && endY != (startY + changeY))
            {
                startY += changeY;
                startX += changeX;
                if (Board[startX, startY] != null)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
