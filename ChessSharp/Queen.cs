using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Queen : IPiece
    {
        private Type type;
        private bool white;
        private string name;
        public Queen(bool c)
        {
            type = Type.QUEEN;
            white = c;
            if (white)
                name = "white_queen";
            else
                name = "black_queen";
        }
        public void Move(int startX, int startY, int endX, int endY, IPiece[,] board)
        {
            throw new NotImplementedException();
        }

        public bool ValidMove(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);

            if (slope == 0 || endX == startX || slope == 1)
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

        public bool ValidPath(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);

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
