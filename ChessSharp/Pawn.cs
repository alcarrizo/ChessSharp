using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    class Pawn : Piece
    {

        public Pawn(bool c, int x)
        {
            Id = x;
            Color = c;
            firstMove = true;
            enPassant = false;
            if (Color)
                Name = "white_pawn";
            else
                Name = "black_pawn";
        }



        public override bool ValidMove(Point start, Point end, Piece[,] Board)
        {
            double slope = Math.Abs((double)end.Y - (double)start.Y) / Math.Abs((double)end.X - (double)start.X);
            int moveSize = 1;

            // checks if the white piece is moving in a valid way
            if (Color)
            {
                if (end.X == start.X && end.Y > start.Y && Board[end.X, end.Y] == null || slope == 1 && Capture(start, end, Board) && end.Y > start.Y)
                {
                    if (firstMove == true)
                    {
                        moveSize = 2;

                    }
                    if (Math.Abs((double)end.Y - (double)start.Y) <= moveSize && ValidPath(start, end, Board))
                    {
                        firstMove = false;
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
                if (end.X == start.X && end.Y < start.Y && Board[end.X, end.Y] == null || slope == 1 && Capture(start, end, Board) && end.Y < start.Y)
                {
                    if (firstMove == true)
                    {
                        moveSize = 2;

                    }
                    if (Math.Abs((double)end.Y - (double)start.Y) <= moveSize && ValidPath(start, end, Board))
                    {
                        firstMove = false;
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

        public bool Capture(Point start, Point end, Piece[,] Board)
        {
            if (Board[end.X, end.Y] == null || Math.Abs(end.X - start.X) > 1 && Math.Abs(end.Y - start.Y) > 1)
            {
                return false;
            }
            return true;
        }

        public override bool ValidPath(Point start, Point end, Piece[,] Board)
        {
            int changeY = 1;
            if (start.Y > end.Y)
            {
                changeY = -1;
            }

            while (end.Y != (start.Y + changeY))
            {
                start.Y += changeY;
                if (Board[start.X, start.Y] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool firstMove { get; private set; }
        public bool enPassant { get; set; }
    }
}
