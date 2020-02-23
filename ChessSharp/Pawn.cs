using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Pawn : Piece
    {

        private bool firstMove;


        public Pawn(bool c, int x)
        {
            Id = x;
            Type = Type.PAWN;
            Color = c;
            firstMove = true;
            if (Color)
                Name = "white_pawn";
            else
                Name = "black_pawn";
        }



        public override bool ValidMove(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);
            int moveSize = 1;

            // checks if the white piece is moving in a valid way
            if (Color)
            {
                if (slope == 0 && endX > startX && Board[endX, endY] == null || slope == 1 && Capture(startX, startY, endX, endY, Board))
                {
                    if (firstMove == true)
                    {
                        moveSize = 2;
                        firstMove = false;
                    }
                    if (Math.Abs((double)endX - (double)startX) <= moveSize && ValidPath(startX, startY, endX, endY, Board))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            // does the same as the above if but from the perspective of the black piece because it moves in the opposite direction
            else
            {
                if (slope == 0 && endX < startX && Board[endX, endY] == null || slope == 1 && Capture(startX, startY, endX, endY, Board))
                {
                    if (firstMove == true)
                    {
                        moveSize = 2;
                        firstMove = false;
                    }
                    if (Math.Abs((double)endX - (double)startX) <= moveSize && ValidPath(startX, startY, endX, endY, Board))
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
            return false; // if none of the above is true it is an invalid move
        }

        public bool Capture(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            if (Board[endX, endY] == null || Math.Abs(endX - startX) > 1 && Math.Abs(endY - startY) > 1)
            {
                return false;
            }
            return true;
        }

        public override bool ValidPath(int startX, int startY, int endX, int endY, Piece[,] Board)
        {
            int changeX = 1;
            if (startX > endX)
            {
                changeX = -1;
            }

            while (endX != (startX + changeX))
            {
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
