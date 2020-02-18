using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Bishop : IPiece
    {
        private Type type;
        private bool white;
        private string name;

        public Bishop(bool c)
        {
            type = Type.BISHOP;
            white = c;
            if (white)
                name = "white_bishop";
            else
                name = "black_bishop";
        }
        public bool ValidMove(int startX, int startY, int endX, int endY, IPiece[,] Board)
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

        public bool ValidPath(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            //if(endY < startY || endX < startX)
            //{
            int changeX = 1;
            int changeY = 1;
            if(startX > endX)
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
