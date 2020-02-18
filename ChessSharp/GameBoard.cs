using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    internal class GameBoard
    {
        int height = 8;
        int width = 8;
        public GameBoard()
        {
            this.Board = new IPiece[height, width];
            Board[0, 0] = new Rook(true);
            Board[0, 1] = new Knight(true);
            Board[0, 2] = new Bishop(true);
            Board[0, 3] = new Queen(true);
            Board[0, 4] = new King(true);
            Board[0, 5] = new Bishop(true);
            Board[0, 6] = new Knight(true);
            Board[0,7] = new Rook(true);
            for (int i = 0; i < width; i++)
            {
                Board[1,i] = new Pawn(true);
                Board[height - 2, i] = new Pawn(false);
            }
            Board[height - 1, 0] = new Rook(false);
            Board[height - 1, 1] = new Knight(false);
            Board[height - 1, 2] = new Bishop(false);
            Board[height - 1, 3] = new Queen(false);
            Board[height - 1, 4] = new King(false);
            Board[height - 1, 5] = new Bishop(false);
            Board[height - 1, 6] = new Knight(false);
            Board[height - 1, 7] = new Rook(false);


        }

        public bool CheckNull(int x, int y)
        {
            if (Board[x,y] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Move(int startX, int startY, int endX, int endY)
        {

            if (Board[startX, startY].ValidMove(startX, startY, endX, endY, Board))
            {
                Board[endX, endY] = Board[startX, startY];
                Board[startX, startY] = null;
            }

        }

        public IPiece[,] Board { get; private set; }

        internal bool AllyPieces(int startX, int startY, int endX, int endY)
        {
            if (Board[endX, endY] == null)
            {
                return false;
            }
            else
            {
                return Board[startX, startY].GetColor() == Board[endX, endY].GetColor();
            }
        }
    }



}
