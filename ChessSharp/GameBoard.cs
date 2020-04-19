using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Point = System.Drawing.Point;
using System.Windows.Input;

namespace ChessSharp
{
    internal class GameBoard
    {
        int height = 8;
        int width = 8;
        private List<Piece> livePieces;
        private List<Piece> whitePieces;
        private List<Piece> blackPieces;

        public GameBoard(Grid grid, Canvas canvas, RotateTransform ro)
        {
            this.Board = new Piece[height, width];
           /* Board[0, 0] = new Rook(true, 1);
            Board[1, 0] = new Knight(true, 1);
            Board[2, 0] = new Bishop(true, 1);
            Board[3, 0] = new King(true, 1);
            Board[4, 0] = new Queen(true, 1);
            Board[5, 0] = new Bishop(true, 2);
            Board[6, 0] = new Knight(true, 2);
            Board[7, 0] = new Rook(true, 2);
            for (int i = 0; i < width; i++)
            {
                Board[i, 1] = new Pawn(true, i);
                Board[i, height - 2] = new Pawn(false, i + 8);
            }
            Board[0, height - 1] = new Rook(false, 3);
            Board[1, height - 1] = new Knight(false, 3);
            Board[2, height - 1] = new Bishop(false, 3);
            Board[3, height - 1] = new King(false, 2);
            Board[4, height - 1] = new Queen(false, 2);
            Board[5, height - 1] = new Bishop(false, 4);
            Board[6, height - 1] = new Knight(false, 4);
            Board[7, height - 1] = new Rook(false, 4);
            */
            
            Board[3, 0] = new King(true, 1);
            Board[3, 1] = new Bishop(false, 1);
            //Board[6, 1] = new Bishop(true, 1);
            //Board[4, 1] = new Knight(false, 1);
            Board[3, height - 1] = new King(false, 2);
            Board[5, height - 2] = new Pawn(true, 2);

            this.Grid = grid;
            this.Canvas = canvas;

            Game = new Game(3, 0, 3, height - 1, canvas, Grid);



            Images = new ImageSelector();
            Ro = ro;
            ID = 5;

            livePieces = new List<Piece>();
            whitePieces = new List<Piece>();
            blackPieces = new List<Piece>();

            getPieces();

        }

        private void getPieces()
        {
            for (int i = 0; i < Grid.ColumnDefinitions.Count; i++)
            {
                for (int j = 0; j < Grid.RowDefinitions.Count; j++)
                {
                    if (Board[i, j] != null)
                    {
                        // white Pieces
                        if (Board[i, j].Color == true)
                        {
                            whitePieces.Add(Board[i, j]);
                            livePieces.Add(Board[i, j]);
                        }
                        //black Pieces
                        else
                        {
                            blackPieces.Add(Board[i, j]);
                            livePieces.Add(Board[i, j]);
                        }
                    }
                }

            }
        }

        public void RemovePiece(Piece temp, Movement moveInfo)
        {
            Game.RemovePieces(temp, moveInfo, livePieces, whitePieces, blackPieces, Board);

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

        public bool ControlPiece(Point start, Player currPlay)
        {
            return currPlay.Color == Board[start.X, start.Y].Color;
        }

        public bool Move(Point start, Point end, Grid Highlights, Player currPlay, Movement moveInfo)
        {
            Piece tempPiece = null;
            bool move = false;
            if (Board[end.X, end.Y] != null && Board[start.X, start.Y].Color != Board[end.X, end.Y].Color)
            {
                tempPiece = Board[end.X, end.Y];
            }
            if (Game.Move(start, end, Board, Highlights, moveInfo))
            {
                move = true;
                if (tempPiece != null)
                {
                    RemovePiece(tempPiece, moveInfo);
                }
            }
            return move;
        }

        public bool EnemyChecks(Point end, Grid Highlights, Movement moveInfo)
        {
            return Game.EnemyChecks(end, Highlights, Board, moveInfo);
        }



        public void ChangePiece(Point end, string name,Movement moveInfo)
        {
            name = name.ToUpper();
            Piece tempPiece = Board[end.X, end.Y];
            switch (name)
            {
                case "QUEEN":
                    {
                        Board[end.X, end.Y] = new Queen(Board[end.X, end.Y].Color, ID);
                    }
                    break;
                case "ROOK":
                    {
                        Board[end.X, end.Y] = new Rook(Board[end.X, end.Y].Color, ID);
                    }
                    break;
                case "KNIGHT":
                    {
                        Board[end.X, end.Y] = new Knight(Board[end.X, end.Y].Color, ID);
                    }
                    break;
                case "BISHOP":
                    {
                        Board[end.X, end.Y] = new Bishop(Board[end.X, end.Y].Color, ID);
                    }
                    break;
            }

            livePieces.Remove(tempPiece);
            livePieces.Add(Board[end.X, end.Y]);

            if (tempPiece.Color == true)
            {
                whitePieces.Remove(tempPiece);
                whitePieces.Add(Board[end.X, end.Y]);
            }
            else
            {
                blackPieces.Remove(tempPiece);
                blackPieces.Add(Board[end.X, end.Y]);
            }


            bool tempBool = false;

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                tempBool = Game.InsufficientMaterial(livePieces, whitePieces, blackPieces, Board);
            });


            if (tempBool)
            {
                moveInfo.Draw = true;
            }

            ID++;
        }

        public bool IsPawn(Point end)
        {
            return Board[end.X, end.Y] is Pawn;
        }


        public void Update()
        {
            Grid.Children.Clear();

            System.Windows.Controls.Image image = new System.Windows.Controls.Image();


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] == null)
                        image = GetImage(Board[i, j], false);
                    else
                        image = GetImage(Board[i, j], Board[i, j].Color);
                    // Grid.SetZIndex(image, 4);
                    Grid.SetColumn(image, i);
                    Grid.SetRow(image, j);

                    image.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5); // sets the rotation point of the image to the center of the image
                    image.RenderTransform = Ro;         // Applies the relevant rotation
                                                        //image.RenderTransform += ro;         // Applies the relevant rotation


                    Grid.Children.Add(image);
                }
            }
        }

        private System.Windows.Controls.Image GetImage(Piece piece, bool white)
        {
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();

            if (piece == null)
            {
                image.Source = Images.Get("empty");
            }
            else
            {
                image.Source = Images.Get(piece.Name);
            }

            return image;
        }

        public void UpdateEnemyPieces(Movement moveInfo, Grid Highlights)
        {

            Game.UpdateEnemyPieces(moveInfo, Board, Highlights, livePieces, whitePieces, blackPieces);

        }

        public Piece[,] Board { get; private set; }
        public Game Game { get; private set; }
        public Grid Grid { get; private set; }

        public Canvas Canvas { get; private set; }
        public Grid Promote { get; set; }
        public ImageSelector Images { get; private set; }

        public int ID { get; private set; }
        public RotateTransform Ro { get; private set; }
    }

}
