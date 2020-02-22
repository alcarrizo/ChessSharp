using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessSharp
{
    internal class GameBoard
    {
        int height = 8;
        int width = 8;

        // keeps track of the position for the kings, used to check if any move puts the king in check
        int whiteKingX;
        int whiteKingY;
        int blackKingX;
        int blackKingY;
        public GameBoard()
        {
            this.Board = new Piece[height, width];
            Board[0, 0] = new Rook(true, 1);
            Board[0, 1] = new Knight(true, 1);
            Board[0, 2] = new Bishop(true, 1);
            Board[0, 3] = new Queen(true, 1);
            Board[0, 4] = new King(true, 1);
            Board[0, 5] = new Bishop(true, 2);
            Board[0, 6] = new Knight(true, 2);
            Board[0, 7] = new Rook(true, 2);
            for (int i = 0; i < width; i++)
            {
                Board[1, i] = new Pawn(true, i);
                Board[height - 2, i] = new Pawn(false, i + 8);
            }
            Board[height - 1, 0] = new Rook(false, 3);
            Board[height - 1, 1] = new Knight(false, 3);
            Board[height - 1, 2] = new Bishop(false, 3);
            Board[height - 1, 3] = new Queen(false, 2);
            Board[height - 1, 4] = new King(false, 2);
            Board[height - 1, 5] = new Bishop(false, 4);
            Board[height - 1, 6] = new Knight(false, 4);
            Board[height - 1, 7] = new Rook(false, 4);

            whiteKingX = 0;
            whiteKingY = 4;
            blackKingX = height - 1;
            blackKingY = 4;


        }

        public bool CheckNull(int x, int y)
        {
            if (Board[x, y] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Move(int startX, int startY, int endX, int endY, Grid Highlights)
        {

            if (Board[startX, startY].ValidMove(startX, startY, endX, endY, Board))
            {
                if (Board[startX, startY].Type == Type.KING)
                {
                    if (Board[startX, startY].Color == true)
                    {
                        whiteKingX = endX;
                        whiteKingY = endY;
                    }

                    else if (Board[startX, startY].Color == false)
                    {
                        blackKingX = endX;
                        blackKingY = endY;
                    }
                }

                Piece temp = Board[endX, endY]; // keeps the piece that is being moved over to revert the pieces back if needed 
                Board[endX, endY] = Board[startX, startY];
                Board[startX, startY] = null;

                if (!AllyKinginCheck(startX, startY, endX, endY, temp, Highlights))
                {
                    EnemyKinginCheck(endX, endY, Highlights);
                }

            }

        }

        private bool AllyKinginCheck(int startX, int startY, int endX, int endY, Piece temp, Grid Highlights)
        {
            Rectangle rect;
            int index;

            if (Board[endX, endY].Color == true)
            {
                if (KingInCheck(whiteKingX, whiteKingY, Board))
                {
                    Board[startX, startY] = Board[endX, endY];
                    Board[endX, endY] = temp;
                    if (Board[startX, startY].Type == Type.KING)
                    {

                        whiteKingX = startX;
                        whiteKingY = startY;

                    }
                    return true;
                }
                else
                {
                    index = (8 * whiteKingX) + whiteKingY;
                    rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }
                }

            }

            else if (Board[endX, endY].Color == false)
            {
                if (KingInCheck(blackKingX, blackKingY, Board))
                {
                    Board[startX, startY] = Board[endX, endY];
                    Board[endX, endY] = temp;

                    if (Board[startX, startY].Type == Type.KING)
                    {

                        blackKingX = startX;
                        blackKingY = startY;

                    }
                    return true;
                }
                else
                {
                    index = (8 * blackKingX) + blackKingY;
                    rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }
                }



            }
            return false;
        }

        private void EnemyKinginCheck(int endX, int endY, Grid Highlights)
        {
            Rectangle rect;
            int index;
            if (Board[endX, endY].Color == true)
            {
                index = (8 * blackKingX) + blackKingY;
                if (KingInCheck(blackKingX, blackKingY, Board))
                {
                    rect = (Rectangle)Highlights.Children[index];
                    rect.Fill = Brushes.Red;

                }
                else
                {
                    rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }

                }
            }

            if (Board[endX, endY].Color == false)
            {
                index = (8 * whiteKingX) + whiteKingY;
                if (KingInCheck(whiteKingX, whiteKingY, Board))
                {

                    rect = (Rectangle)Highlights.Children[index];
                    rect.Fill = Brushes.Red;
                }
                else
                {
                    rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }
                }
            }
        }


        public Piece[,] Board { get; private set; }

        internal bool AllyPieces(int startX, int startY, int endX, int endY)
        {
            if (Board[endX, endY] == null)
            {
                return false;
            }
            else
            {
                return Board[startX, startY].Color == Board[endX, endY].Color;
            }
        }



        public bool KingInCheck(int endX, int endY, Piece[,] Board)
        {
            bool check = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null && Board[i, j].Color != Board[endX, endY].Color)
                    {
                        if (Board[i, j].Type == Type.PAWN)
                        {
                            double slope = Math.Abs((double)endY - (double)j) / Math.Abs((double)endX - (double)i);

                            if (slope == 1 && Math.Abs(endX - i) <= 1 && Math.Abs(endY - j) <= 1)
                            {
                                if (Board[i, j].Color && endX > i)
                                {
                                    check = true;
                                }
                                else if (!Board[i, j].Color && endX < i)
                                {
                                    check = true;
                                }
                            }


                        }
                        else if (Board[i, j].ValidMove(i, j, endX, endY, Board))
                        {
                            check = true;
                        }
                    }
                }
            }
            return check;
        }

    }




}
