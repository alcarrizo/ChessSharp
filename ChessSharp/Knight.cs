using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class Knight : IPiece
    {
        private Type type;
        private bool white;
        private string name;
        public Knight(bool c)
        {
            type = Type.KNIGHT;
            white = c;
            if (white)
                name = "white_knight";
            else
                name = "black_knight";
        }
        public bool ValidMove(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            double slope = Math.Abs((double)endY - (double)startY) / Math.Abs((double)endX - (double)startX);

            if (slope == 0.5 || slope == 2)
            {
                if (Math.Abs((double)endY - (double)startY) <= 2 && Math.Abs((double)endX - (double)startX) <= 2)
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
            throw new NotImplementedException();
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
