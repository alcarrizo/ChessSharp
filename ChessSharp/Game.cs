using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Drawing.Point;


namespace ChessSharp
{
    class Game
    {
        int height = 8;
        int width = 8;

        Point whiteKing;
        Point blackKing;


        public Game(int wkx, int wky, int bkx, int bky)
        {
            whiteKing.X = wkx;
            whiteKing.Y = wky;
            blackKing.X = bkx;
            blackKing.Y = bky;
            tempPawn = null;
        }


        public bool Move(Point start, Point end, Piece[,] Board, Grid Highlights, Movement moveInfo)
        {
            bool enPassant = false;
            bool move = false;
            if (!AllyPieces(start, end, Board) && !AllyKinginCheck(start, end, Board, Highlights))
            {
                // checks for a standard move
                if (Board[start.X, start.Y].ValidMove(start, end, Board))
                {
                    move = true;
                    if (Board[start.X, start.Y] is King)
                    {

                        if (Board[start.X, start.Y].Color == true)
                        {
                            whiteKing.X = end.X;
                            whiteKing.Y = end.Y;
                        }

                        else
                        {
                            blackKing.X = end.X;
                            blackKing.Y = end.Y;
                        }
                        King tempKing = Board[start.X, start.Y] as King;
                        tempKing.firstMove = false;
                    }
                    else if (Board[start.X, start.Y] is Rook)
                    {
                        Rook tempRook = Board[start.X, start.Y] as Rook;
                        tempRook.firstMove = false;
                    }
                    else if (Board[start.X, start.Y] is Pawn)
                    {
                        if (Math.Abs(start.Y - end.Y) == 2 && ((start.X > 0 && Board[end.X - 1, end.Y] is Pawn) || (start.X < width - 1 && Board[end.X + 1, end.Y] is Pawn)))
                        {
                            if (tempPawn != null)
                            {
                                if (tempPawn.enPassant == true)
                                {
                                    tempPawn.enPassant = false;
                                }
                            }
                            enPassant = true;
                            tempPawn = Board[start.X, start.Y] as Pawn;
                            tempPawn.enPassant = true;

                        }
                    }


                }
                //checks if the player is trying to castle their king
                else if (Board[start.X, start.Y] is King && IsCastling(start, end, Board, moveInfo))
                {
                    move = true;
                    if (Board[start.X, start.Y].Color == true)
                    {
                        whiteKing.X = end.X;
                        whiteKing.Y = end.Y;
                    }

                    else
                    {
                        blackKing.X = end.X;
                        blackKing.Y = end.Y;
                    }
                    moveInfo.castling = true;
                }
                // checks for en passant
                else if (Board[start.X, start.Y] is Pawn && IsEnPassant(start, end, Board, moveInfo))
                {
                    move = true;
                    moveInfo.enPassant = true;
                }

                if (move == true)
                {

                    Board[end.X, end.Y] = Board[start.X, start.Y];
                    Board[start.X, start.Y] = null;

                    if (enPassant == false && tempPawn != null)
                    {
                        if (tempPawn.enPassant == true)
                        {
                            tempPawn.enPassant = false;
                        }
                        tempPawn = null;
                    }
                }
            }

            return move;
        }

        private bool IsEnPassant(Point start, Point end, Piece[,] board, Movement moveInfo)
        {
            bool capture = false;
            Pawn temp;
            double slope = Math.Abs(((double)end.Y - (double)start.Y) / ((double)end.X - (double)start.X));
            if (slope == 1 && Math.Abs(end.X - start.X) == 1 && Math.Abs(end.Y - start.Y) == 1)
                if (board[start.X, start.Y].Color == true && end.Y > start.Y)
                {
                    if (board[end.X, end.Y - 1] != null && board[end.X, end.Y - 1].Color != board[start.X, start.Y].Color)
                    {
                        temp = board[end.X, end.Y - 1] as Pawn;
                        if (temp.enPassant == true)
                        {
                            moveInfo.pawnX = end.X;
                            moveInfo.pawnY = end.Y - 1;

                            capture = true;
                            board[end.X, end.Y - 1] = null;
                        }
                    }

                }
                else if (board[start.X, start.Y].Color == false && end.Y < start.Y)
                {
                    if (board[end.X, end.Y + 1] != null && board[end.X, end.Y + 1].Color != board[start.X, start.Y].Color)
                    {
                        temp = board[end.X, end.Y + 1] as Pawn;
                        if (temp.enPassant == true)
                        {
                            moveInfo.pawnX = end.X;
                            moveInfo.pawnY = end.Y + 1;

                            capture = true;
                            board[end.X, end.Y + 1] = null;
                        }
                    }

                }
            return capture;
        }

        public bool IsCastling(Point start, Point end, Piece[,] Board, Movement moveInfo)
        {
            bool castling = false;
            bool clearSpace = true;
            King tempKing = Board[start.X, start.Y] as King;
            Rook tempRook = null;


            if (start.X - end.X == -2 && start.Y == end.Y)
            {
                if (Board[0, start.Y] != null && Board[0, start.Y] is Rook)
                {
                    tempRook = Board[width - 1, start.Y] as Rook;

                    if (tempKing.firstMove == true && tempRook.firstMove == true)
                    {
                        for (int i = start.X + 1; i < width - 1; i++)
                        {
                            if (Board[i, start.Y] != null || (i <= end.X && Capture(new Point(i, start.Y), Board[start.X, start.Y].Color, Board)))
                            {
                                clearSpace = false;
                            }
                        }

                    }
                    else
                    {
                        clearSpace = false;
                    }
                }
                if (clearSpace == true)
                {
                    moveInfo.rookStartX = width - 1;
                    moveInfo.rookStartY = start.Y;
                    moveInfo.rookEndX = end.X - 1;
                    moveInfo.rookEndY = start.Y;

                    Board[end.X - 1, start.Y] = Board[width - 1, start.Y];
                    Board[width - 1, start.Y] = null;
                    castling = true;
                    tempRook.firstMove = false;
                    tempKing.firstMove = false;
                }
            }

            else if (start.X - end.X == 2 && start.Y == end.Y)
            {
                if (Board[0, start.Y] != null && Board[0, start.Y] is Rook)
                {
                    tempRook = Board[0, start.Y] as Rook;

                    if (tempKing.firstMove == true && tempRook.firstMove == true)
                    {
                        for (int i = start.X - 1; i > 0; i--)
                        {
                            if (Board[i, start.Y] != null || (i >= end.X && Capture(new Point(i, start.Y), Board[start.X, start.Y].Color, Board)))
                            {
                                clearSpace = false;
                            }
                        }

                    }
                    else
                    {
                        clearSpace = false;
                    }
                }


                if (clearSpace == true)
                {
                    moveInfo.rookStartX = 0;
                    moveInfo.rookStartY = start.Y;
                    moveInfo.rookEndX = end.X + 1;
                    moveInfo.rookEndY = start.Y;

                    Board[end.X + 1, start.Y] = Board[0, start.Y];
                    Board[0, start.Y] = null;
                    castling = true;
                    tempRook.firstMove = false;
                    tempKing.firstMove = false;
                }
            }



            return castling;
        }

        public bool EnemyChecks(Point end, Grid Highlights, Piece[,] Board, Movement moveInfo)
        {
            bool mate = false;

            if (EnemyKinginCheck(end, Highlights, Board))
            {
                moveInfo.check = true;

                checkPieces.Clear();
                FindCheckPieces(Board[end.X, end.Y].Color, Board);

                foreach (var point in checkPieces.ToList())
                {
                    if (CheckMate(point, Board, Highlights))
                    {
                        moveInfo.checkMate = true;
                        mate = true;
                    }
                }
            }

            return mate;
        }



        public bool AllyKinginCheck(Point start, Point end, Piece[,] Board, Grid Highlights)
        {

            System.Windows.Shapes.Rectangle rect;
            int index;

            if (Board[start.X, start.Y] is King)
            {

                if (Board[start.X, start.Y].Color == true)
                {
                    whiteKing.X = end.X;
                    whiteKing.Y = end.Y;
                }

                else if (Board[start.X, start.Y].Color == false)
                {
                    blackKing.X = end.X;
                    blackKing.Y = end.Y;
                }
            }

            Piece temp = Board[end.X, end.Y]; // keeps the piece that is being moved over to revert the pieces back if needed 
            Board[end.X, end.Y] = Board[start.X, start.Y];
            Board[start.X, start.Y] = null;

            if (Board[end.X, end.Y].Color == true)
            {
                if (Capture(whiteKing, true, Board))
                {
                    Board[start.X, start.Y] = Board[end.X, end.Y];
                    Board[end.X, end.Y] = temp;
                    if (Board[start.X, start.Y] is King)
                    {

                        whiteKing.X = start.X;
                        whiteKing.Y = start.Y;

                    }
                    return true;
                }
                else
                {

                    Board[start.X, start.Y] = Board[end.X, end.Y];
                    Board[end.X, end.Y] = temp;
                    if (Board[start.X, start.Y] is King)
                    {

                        whiteKing.X = start.X;
                        whiteKing.Y = start.Y;

                    }

                    index = (8 * whiteKing.X) + whiteKing.Y;
                    rect = (System.Windows.Shapes.Rectangle)Highlights.Children[index];
                    if (rect.Fill == System.Windows.Media.Brushes.Red)
                    {
                        rect.Fill = System.Windows.Media.Brushes.Transparent;
                    }

                    return false;
                }

            }

            else if (Board[end.X, end.Y].Color == false)
            {
                if (Capture(blackKing, false, Board))
                {
                    Board[start.X, start.Y] = Board[end.X, end.Y];
                    Board[end.X, end.Y] = temp;

                    if (Board[start.X, start.Y] is King)
                    {

                        blackKing.X = start.X;
                        blackKing.Y = start.Y;

                    }
                    return true;
                }
                else
                {

                    Board[start.X, start.Y] = Board[end.X, end.Y];
                    Board[end.X, end.Y] = temp;
                    if (Board[start.X, start.Y] is King)
                    {

                        blackKing.X = start.X;
                        blackKing.Y = start.Y;

                    }

                    index = (8 * blackKing.X) + blackKing.Y;
                    rect = (System.Windows.Shapes.Rectangle)Highlights.Children[index];
                    if (rect.Fill == System.Windows.Media.Brushes.Red)
                    {
                        rect.Fill = System.Windows.Media.Brushes.Transparent;
                    }


                    return false;
                }



            }


            return false;
        }

        private bool EnemyKinginCheck(Point end, Grid Highlights, Piece[,] Board)
        {
            System.Windows.Shapes.Rectangle rect;
            int index;
            Point king = new Point(0, 0);
            if (Board[end.X, end.Y].Color == true)
            {
                king.X = blackKing.X;
                king.Y = blackKing.Y;
            }
            else
            {
                king.X = whiteKing.X;
                king.Y = whiteKing.Y;
            }

            index = (8 * king.X) + king.Y;
            if (Capture(king, Board[king.X, king.Y].Color, Board))
            {
                rect = (System.Windows.Shapes.Rectangle)Highlights.Children[index];
                rect.Fill = System.Windows.Media.Brushes.Red;

                return true;
            }
            else
            {
                rect = (System.Windows.Shapes.Rectangle)Highlights.Children[index];
                if (rect.Fill == System.Windows.Media.Brushes.Red)
                {
                    rect.Fill = System.Windows.Media.Brushes.Transparent;
                }

            }
            return false;
        }


        private bool CheckMate(Point point, Piece[,] Board, Grid Highlights)
        {
            bool checkmate = true;

            Point king = new Point(0, 0);

            if (StopCheck(point, Highlights, Board))
            {
                checkmate = false;
            }

            if (Board[point.X, point.Y].Color == true)
            {
                king.X = blackKing.X;
                king.Y = blackKing.Y;
            }
            else
            {
                king.X = whiteKing.X;
                king.Y = whiteKing.Y;
            }

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {

                    if (king.X + i >= 0 && king.X + i < width && king.Y + j >= 0 && king.Y + j < height)
                    {

                        if (!Capture(new Point(king.X + i, king.Y + j), Board[king.X, king.Y].Color, Board) && !AllyPieces(king, new Point(king.X + i, king.Y + j), Board)
                        && Board[king.X, king.Y].ValidMove(king, new Point(king.X + i, king.Y + j), Board))
                        {
                            checkmate = false;
                        }

                    }
                }


            }
            return checkmate;
        }
        private bool StopCheck(Point point, Grid Highlights, Piece[,] Board)
        {
            bool stopped = false;
            System.Windows.Shapes.Rectangle rect;
            int index;
            Point king = new Point(0, 0);

            //checking if the king can capture the piece that is putting it in check
            if (Board[point.X, point.Y].Color == true)
            {
                king.X = blackKing.X;
                king.Y = blackKing.Y;

            }
            else if (Board[point.X, point.Y].Color == false)
            {
                king.X = whiteKing.X;
                king.Y = whiteKing.Y;

            }

            if (Board[king.X, king.Y].ValidMove(king, point, Board) && !AllyKinginCheck(king, point, Board, Highlights))
            {
                stopped = true;
            }

            // checks if you can capture the piece that is putting the king in check
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null && Board[i, j].Color != Board[point.X, point.Y].Color && Board[i, j].ValidMove(new Point(i, j), point, Board) && !AllyKinginCheck(new Point(i, j), point, Board, Highlights))
                    {
                        stopped = true;

                    }
                }
            }

            if (!(Board[point.X, point.Y] is Knight) && stopped == false)
            {

                int yMove = 0; // x movement
                int xMove = 0; // y movement
                int tempX = point.X;
                int tempY = point.Y;

                if (point.X > king.X)
                {
                    xMove = -1;
                }
                else if (point.X < king.X)
                {
                    xMove = 1;
                }

                // used to take into account the change in the y coordinate for the different movements when moving through the array
                if (point.Y > king.Y)
                {
                    yMove = -1;
                }
                else if (point.Y < king.Y)
                {
                    yMove = 1;
                }

                while (tempX + xMove != king.X || tempY + yMove != king.Y)
                {
                    tempX = tempX + xMove;
                    tempY = tempY + yMove;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Board[i, j] != null)
                            {
                                if (!(Board[i, j] is King) && Board[i, j].Color != Board[point.X, point.Y].Color
                                    && Board[i, j].ValidMove(new Point(i, j), new Point(tempX, tempY), Board)
                                    && !AllyKinginCheck(new Point(i, j), new Point(tempX, tempY), Board, Highlights))
                                {
                                    stopped = true;

                                }
                            }

                        }
                    }
                }


            }

            // reapplies the red highlight on the king that got removed by AllyKingInCheck

            index = (8 * king.X) + king.Y;

            rect = (System.Windows.Shapes.Rectangle)Highlights.Children[index];
            rect.Fill = System.Windows.Media.Brushes.Red;


            return stopped;
        }

        internal bool AllyPieces(Point start, Point end, Piece[,] Board)
        {
            if (Board[end.X, end.Y] == null)
            {
                return false;
            }
            else
            {
                return Board[start.X, start.Y].Color == Board[end.X, end.Y].Color;
            }
        }

        //holds the coordinates of the pieces putting the king in check
        private void FindCheckPieces(bool color, Piece[,] Board)
        {
            Point king = new Point(0, 0);
            if (color == true)
            {
                king.X = blackKing.X;
                king.Y = blackKing.Y;
            }
            else
            {
                king.X = whiteKing.X;
                king.Y = whiteKing.Y;
            }
            Capture(king, Board[(int)king.X, (int)king.Y].Color, Board);
        }

        public bool Capture(Point point, bool color, Piece[,] Board)
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
                        if (Board[i, j] is Pawn)
                        {
                            double slope = Math.Abs((double)point.Y - (double)j) / Math.Abs((double)point.X - (double)i);

                            if (slope == 1 && Math.Abs((double)point.X - (double)i) <= 1 && Math.Abs((double)point.Y - (double)j) <= 1)
                            {
                                if (Board[i, j].Color && point.Y > j)
                                {
                                    check = true;
                                    checkPiece = true;
                                }
                                else if (!Board[i, j].Color && point.Y < j)
                                {
                                    check = true;
                                    checkPiece = true;
                                }
                                if (checkPiece == true && Board[point.X, point.Y] != null && Board[point.X, point.Y] is King)
                                {
                                    checkPieces.Add(new Point(i, j));
                                }
                            }


                        }
                        else if (Board[i, j].ValidMove(new Point(i, j), point, Board))
                        {
                            check = true;
                            if (Board[point.X, point.Y] != null && Board[point.X, point.Y] is King)
                            {
                                checkPieces.Add(new Point(i, j));
                            }
                        }
                    }
                }
            }
            return check;
        }


        public void UpdateEnemyPieces(Movement moveInfo, Piece[,] Board, Grid Highlights)
        {
            Pawn tempPawn2 = null;

            //castling
            if (moveInfo.castling == true)
            {
                Board[moveInfo.rookEndX, moveInfo.rookEndY] = Board[moveInfo.rookStartX, moveInfo.rookStartY];
                Board[moveInfo.rookStartX, moveInfo.rookStartY] = null;

                Board[moveInfo.endX, moveInfo.endY] = Board[moveInfo.startX, moveInfo.startY];
                Board[moveInfo.startX, moveInfo.startY] = null;

                if (Board[moveInfo.endX, moveInfo.endY].Color == true)
                {
                    whiteKing.X = moveInfo.endX;
                    whiteKing.Y = moveInfo.endY;
                }
                else
                {
                    blackKing.X = moveInfo.endX;
                    blackKing.Y = moveInfo.endY;
                }
            }
            //enpassant
            else if (moveInfo.enPassant == true)
            {
                Board[moveInfo.endX, moveInfo.endY] = Board[moveInfo.startX, moveInfo.startY];
                Board[moveInfo.startX, moveInfo.startY] = null;

                Board[moveInfo.pawnX, moveInfo.pawnY] = null;
            }
            //promotion
            else if (moveInfo.promotion == true)
            {
                switch (moveInfo.pawnEvolvesTo)
                {
                    case "Queen":
                        Board[moveInfo.endX, moveInfo.endY] = new Queen(Board[moveInfo.startX, moveInfo.startY].Color, 50);
                        break;
                    case "Knight":
                        Board[moveInfo.endX, moveInfo.endY] = new Knight(Board[moveInfo.startX, moveInfo.startY].Color, 50);
                        break;
                    case "Bishop":
                        Board[moveInfo.endX, moveInfo.endY] = new Bishop(Board[moveInfo.startX, moveInfo.startY].Color, 50);
                        break;
                    case "Rook":
                        Board[moveInfo.endX, moveInfo.endY] = new Rook(Board[moveInfo.startX, moveInfo.startY].Color, 50);
                        break;
                }

                Board[moveInfo.startX, moveInfo.startY] = null;
            }
            //regular move
            else
            {
                

                if(Board[moveInfo.startX, moveInfo.startY] is King)
                {
                    if(Board[moveInfo.startX, moveInfo.startY].Color == true)
                    {
                        whiteKing.X = moveInfo.endX;
                        whiteKing.Y = moveInfo.endY;
                    }
                    else
                    {
                        blackKing.X = moveInfo.endX;
                        blackKing.Y = moveInfo.endY;
                    }

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Rectangle rect = (System.Windows.Shapes.Rectangle)Highlights.Children[(8 * moveInfo.startX) + moveInfo.startY];
                        if (rect.Fill == Brushes.Red)
                        {
                            rect.Fill = System.Windows.Media.Brushes.Transparent;
                        }
                    });

                }
                else if (Board[moveInfo.startX, moveInfo.startY] is Pawn)
                {
                    
                    if (Math.Abs(moveInfo.startY - moveInfo.endY) == 2 && ((moveInfo.startX > 0 && Board[moveInfo.endX - 1, moveInfo.endY] is Pawn) || (moveInfo.startX < width - 1 && Board[moveInfo.endX + 1, moveInfo.endY] is Pawn)))
                    {
                        if (tempPawn2 != null)
                        {
                            if (tempPawn2.enPassant == true)
                            {
                                tempPawn2.enPassant = false;
                            }
                        }
                        tempPawn2 = Board[moveInfo.startX, moveInfo.startY] as Pawn;
                        tempPawn2.enPassant = true;

                    }
                }
                else
                {
                    if (tempPawn2 != null)
                    {
                        if (tempPawn2.enPassant == true)
                        {
                            tempPawn2.enPassant = false;
                        }
                        tempPawn2 = null;
                    }
                }

                Board[moveInfo.endX, moveInfo.endY] = Board[moveInfo.startX, moveInfo.startY];
                Board[moveInfo.startX, moveInfo.startY] = null;
            }

            // checks
            if (moveInfo.check == true)
            {
                Point king = new Point();
                //Rectangle rect = new Rectangle();
                if (Board[moveInfo.endX, moveInfo.endY].Color == true)
                {
                    king = blackKing;
                }
                else
                {
                    king = whiteKing;
                }

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Rectangle rect = (System.Windows.Shapes.Rectangle)Highlights.Children[(8 * king.X) + king.Y];
                    rect.Fill = System.Windows.Media.Brushes.Red;
                });

            }
            else
            {
                Point king = new Point();
                //Rectangle rect = new Rectangle();
                if (Board[moveInfo.endX, moveInfo.endY].Color == true)
                {
                    king = whiteKing;
                }
                else
                {
                    king = blackKing;
                }
                

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Rectangle rect = (System.Windows.Shapes.Rectangle)Highlights.Children[(8 * king.X) + king.Y];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = System.Windows.Media.Brushes.Transparent;
                    }
                });
            }

        }


        public List<Point> checkPieces = new List<Point>();

        public Pawn tempPawn { get; set; }


    }

}
