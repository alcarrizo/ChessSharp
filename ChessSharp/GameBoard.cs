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
            bool mate = false;

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

                Board[endX, endY] = Board[startX, startY];
                Board[startX, startY] = null;


                if (EnemyKinginCheck(endX, endY, Highlights))
                {
                    checkPieces.Clear();
                    FindCheckPieces(Board[endX, endY].Color);

                    foreach (var point in checkPieces.ToList())
                    {
                        if (CheckMate((int)point.X, (int)point.Y, Highlights))
                        {
                            mate = true;
                        }
                    }
                    if (mate == true)
                    {
                        MessageBox.Show("Checkmate");
                    }
                }


            }

        }


        public bool AllyKinginCheck(int startX, int startY, int endX, int endY, Grid Highlights)
        {

            Rectangle rect;
            int index;

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

            if (Board[endX, endY].Color == true)
            {
                if (Capture(whiteKingX, whiteKingY, true))
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

                    Board[startX, startY] = Board[endX, endY];
                    Board[endX, endY] = temp;
                    if (Board[startX, startY].Type == Type.KING)
                    {

                        whiteKingX = startX;
                        whiteKingY = startY;

                    }

                    index = (8 * whiteKingX) + whiteKingY;
                    rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }

                    return false;
                }

            }

            else if (Board[endX, endY].Color == false)
            {
                if (Capture(blackKingX, blackKingY, false))
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

                    Board[startX, startY] = Board[endX, endY];
                    Board[endX, endY] = temp;
                    if (Board[startX, startY].Type == Type.KING)
                    {

                        blackKingX = startX;
                        blackKingY = startY;

                    }

                    index = (8 * blackKingX) + blackKingY;
                    rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }


                    return false;
                }



            }


            return false;
        }

        private bool EnemyKinginCheck(int endX, int endY, Grid Highlights)
        {
            Rectangle rect;
            int index;
            int kingX = 0;
            int kingY = 0;
            if (Board[endX, endY].Color == true)
            {
                kingX = blackKingX;
                kingY = blackKingY;
            }
            else
            {
                kingX = whiteKingX;
                kingY = whiteKingY;
            }

            index = (8 * kingX) + kingY;
            if (Capture(kingX, kingY, Board[kingX, kingY].Color))
            {
                rect = (Rectangle)Highlights.Children[index];
                rect.Fill = Brushes.Red;

                return true;
            }
            else
            {
                rect = (Rectangle)Highlights.Children[index];
                if (rect.Fill == Brushes.Red)
                {
                    rect.Fill = Brushes.Transparent;
                }

            }
            return false;
        }

        private bool CheckMate(int x, int y, Grid Highlights)
        {
            bool checkmate = true;

            int kingX = 0;
            int kingY = 0;

            if (StopCheck(x, y, Highlights))
            {
                checkmate = false;
            }

            if (Board[x, y].Color == true)
            {
                kingX = blackKingX;
                kingY = blackKingY;
            }
            else
            {
                kingX = whiteKingX;
                kingY = whiteKingY;
            }

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {

                    if (kingX + i >= 0 && kingX + i < width && kingY + j >= 0 && kingY + j < height)
                    {

                        if (!Capture(kingX + i, kingY + j, Board[kingX, kingY].Color) && !AllyPieces(kingX, kingY, kingX + i, kingY + j)
                        && Board[kingX, kingY].ValidMove(kingX, kingY, kingX + i, kingY + j, Board))
                        {
                            checkmate = false;
                        }

                    }
                }


            }
            return checkmate;
        }
        private bool StopCheck(int x, int y, Grid Highlights)
        {
            bool stopped = false;
            Rectangle rect;
            int index;
            int kingX = 0;
            int kingY = 0;

            //checking if the king can capture the piece that is putting it in check
            if (Board[x, y].Color == true)
            {
                kingX = blackKingX;
                kingY = blackKingY;

            }
            else if (Board[x, y].Color == false)
            {
                kingX = whiteKingX;
                kingY = whiteKingY;

            }

            if (Board[kingX, kingY].ValidMove(kingX, kingY, x, y, Board) && !AllyKinginCheck(kingX, kingY, x, y, Highlights))
            {
                stopped = true;
            }

            // checks if you can capture the piece that is putting the king in check
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null && Board[i, j].Color != Board[x, y].Color && Board[i, j].ValidMove(i, j, x, y, Board) && !AllyKinginCheck(i, j, x, y, Highlights))
                    {
                        stopped = true;

                    }
                }
            }

            if (Board[x, y].Type != Type.KNIGHT && stopped == false)
            {
                int yMove = 0; // x movement
                int xMove = 0; // y movement
                int tempX = x;
                int tempY = y;

                if (x > kingX)
                {
                    xMove = -1;
                }
                else if (x < kingX)
                {
                    xMove = 1;
                }

                // used to take into account the change in the y coordinate for the different movements when moving through the array
                if (y > kingY)
                {
                    yMove = -1;
                }
                else if (y < kingY)
                {
                    yMove = 1;
                }

                while (tempX + xMove != kingX || tempY + yMove != kingY)
                {
                    tempX = tempX + xMove;
                    tempY = tempY + yMove;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Board[i, j] != null)
                            {
                                if (Board[i, j].Type != Type.KING && Board[i, j].Color != Board[x, y].Color && Board[i, j].ValidMove(i, j, tempX, tempY, Board) && !AllyKinginCheck(i, j, tempX, tempY, Highlights))
                                {
                                    stopped = true;

                                }
                            }

                        }
                    }
                }


            }

            // reapplies the red highlight on the king that got removed by AllyKingInCheck

            index = (8 * kingX) + kingY;

            rect = (Rectangle)Highlights.Children[index];
            rect.Fill = Brushes.Red;


            return stopped;
        }



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

        private void FindCheckPieces(bool color)
        {
            int kingX = 0;
            int kingY = 0;

            if (color == true)
            {
                kingX = blackKingX;
                kingY = blackKingY;
            }
            else
            {
                kingX = whiteKingX;
                kingY = whiteKingY;
            }
            Capture(kingX, kingY, Board[kingX, kingY].Color);
        }


        public bool Capture(int x, int y, bool color)
        {
            bool check = false;
            bool checkPiece;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    checkPiece = false;
                    if (Board[i, j] != null && Board[i, j].Color != color)
                    {
                        if (Board[i, j].Type == Type.PAWN)
                        {
                            double slope = Math.Abs((double)y - (double)j) / Math.Abs((double)x - (double)i);

                            if (slope == 1 && Math.Abs(x - i) <= 1 && Math.Abs(y - j) <= 1)
                            {
                                if (Board[i, j].Color && x > i)
                                {
                                    check = true;
                                    checkPiece = true;
                                }
                                else if (!Board[i, j].Color && x < i)
                                {
                                    check = true;
                                    checkPiece = true;
                                }
                                if (checkPiece == true && Board[x, y] != null && Board[x, y].Type == Type.KING)
                                {
                                    checkPieces.Add(new Point(i, j));
                                }
                            }


                        }
                        else if (Board[i, j].ValidMove(i, j, x, y, Board))
                        {
                            check = true;
                            if (Board[x, y] != null && Board[x, y].Type == Type.KING)
                            {
                                checkPieces.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }
            return check;
        }

        public Piece[,] Board { get; private set; }

        //holds the coordinates of the pieces putting the king in check
        public List<Point> checkPieces = new List<Point>();
    }

}
