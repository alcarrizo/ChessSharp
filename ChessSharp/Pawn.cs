﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Pawn : IPiece
    {
        private Type type;
        private bool firstMove;
        private bool white;
        private string name;

        public Pawn(bool c)
        {
            type = Type.PAWN;
            white = c;
            firstMove = true;
            if (white)
                name = "white_pawn";
            else
                name = "black_pawn";
        }



        public bool ValidMove(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);
            int moveSize = 1;

            // checks if the white piece is moving in a valid way
            if (white)
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

        public bool Capture(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            if(Board[endX,endY] == null || Math.Abs(endX-startX) > 1 && Math.Abs(endY - startY) > 1)
            {
                return false;
            }
            return true;
        }

        public bool ValidPath(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            int changeX = 1;
            if (startX > endX)
            {
                changeX = -1;
            }

            while (endX != (startX + changeX) )
            {
                startX += changeX;
                if (Board[startX, startY] != null)
                {
                    return false;
                }
            }
            return true;
        }

        Type IPiece.GetType()
        {
            return type;
        }

        public bool GetColor()
        {
            return white;
        }

        public string GetName()
        {
            return name;
        }
    }
}