using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    class King : Piece
    {

        public King(bool c, int x)
        {
            Id = x;
            Color = c;
            firstMove = true;
            if (Color)
                Name = "white_king";
            else
                Name = "black_king";
        }



        public override bool ValidMove(Point start, Point end, Piece[,] Board)
        {
            if (Math.Abs((double)end.Y - (double)start.Y) <= 1 && Math.Abs((double)end.X - (double)start.X) <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool ValidPath(Point start, Point end, Piece[,] Board)
        {
            //return KingInCheck(endX, endY, Board) == false;
            return true;
        }


        public bool firstMove { get; set; }


    }
}
