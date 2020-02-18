using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    class King : IPiece
    {
        private Type type;
        private bool white;
        private string name;

        public King(bool c)
        {
            type = Type.KING;
            white = c;
            if (white)
                name = "white_king";
            else
                name = "black_king";
        }
        public bool ValidMove(int startX, int startY, int endX, int endY, IPiece[,] Board)
        {
            if (Math.Abs((double)endY - (double)startY) <= 1 && Math.Abs((double)endX - (double)startX) <= 1 && ValidPath(startX, startY, endX, endY, Board))
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
            return KingInCheck(endX, endY, Board) == false;
        }

        public bool KingInCheck(int endX, int endY, IPiece[,] Board)
        {
            bool check = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null && Board[i,j].GetColor() != this.white)
                    {
                        if (Board[i, j].GetType() == Type.PAWN)
                        {
                            Pawn temp = new Pawn(Board[i, j].GetColor());
                            if (Board[i, j].GetType() == Type.PAWN && temp.Capture(i,j,endX,endY,Board))
                            {
                                check = true;
                            }
                        }
                        if (Board[i, j].ValidMove(i, j, endX, endY, Board))
                        {
                            check = true;
                        }
                    }
                }
            }
            return check;
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
