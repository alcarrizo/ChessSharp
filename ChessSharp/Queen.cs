﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    class Queen : Piece
    {
        public Queen(bool c, int x)
        {
            Id = x;
            Color = c;
            if (Color)
                Name = "white_queen";
            else
                Name = "black_queen";
        }

        public override bool ValidMove(Point start, Point end, Piece[,] Board)
        {
            double slope = Math.Abs((double)end.Y - (double)start.Y) / Math.Abs((double)end.X - (double)start.X);

            if (slope == 0 || end.X == start.X || slope == 1)
            {
                if (ValidPath(start, end, Board))
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
            double slope = Math.Abs((double)end.Y - (double)start.Y) / Math.Abs((double)end.X - (double)start.X);

            int changeX = 1;
            int changeY = 1;

            // used to take into account the change in the x coordinate for the different movements when moving through the array
            if (start.X > end.X)
            {
                changeX = -1;
            }
            else if (start.X == end.X)
            {
                changeX = 0;
            }

            // used to take into account the change in the y coordinate for the different movements when moving through the array
            if (start.Y > end.Y)
            {
                changeY = -1;
            }
            else if (start.Y == end.Y)
            {
                changeY = 0;
            }


            while (end.X != (start.X + changeX) || end.Y != (start.Y + changeY))
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
