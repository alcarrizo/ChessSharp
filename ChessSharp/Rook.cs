using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Rook : Piece
    {

        public Rook(bool c, int x)
        {
            Id = x;
            Type = Type.ROOK;
            Color = c;
            if (Color)
                Name = "white_rook";
            else
                Name = "black_rook";
        }
        public override bool ValidMove(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);

            if (slope == 0 || endX == startX)
            {
                if (ValidPath(startX, startY, endX, endY, Board))
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
            int changeX = 1;
            int changeY = 1;

            // used to take into account the change in the x coordinate for the different movements when moving through the array
            if (startX > endX)
            {
                changeX = -1;
            }
            else if (startX == endX)
            {
                changeX = 0;
            }

            // used to take into account the change in the y coordinate for the different movements when moving through the array
            if (startY > endY)
            {
                changeY = -1;
            }
            else if (startY == endY)
            {
                changeY = 0;
            }

            while (endX != (startX + changeX) || endY != (startY + changeY))
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
