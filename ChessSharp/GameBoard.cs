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

        public GameBoard(Grid grid, RotateTransform ro)
        {
            this.Board = new Piece[height, width];
            Board[0, 0] = new Rook(true, 1);
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

            Game = new Game(3, 0, 3, height - 1);

            Grid = grid;
            Images = new ImageSelector();
            Ro = ro;
            ID = 5;

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

            bool move = false;
            if (Game.Move(start, end, Board, Highlights, moveInfo))
            {
                move = true;
            }
            return move;
        }

        public bool EnemyChecks(Point end, Grid Highlights, Movement moveInfo)
        {
            return Game.EnemyChecks(end, Highlights, Board, moveInfo);
        }



        public void ChangePiece(Point end, string name)
        {
            name = name.ToUpper();
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
            ID++;
        }

        public bool IsPawn(Point end)
        {
            return Board[end.X, end.Y] is Pawn;
        }

        public void Update(Grid FullScreen)
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
                    image.RenderTransform = FullScreen.RenderTransform;         // Applies the relevant rotation
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


        public Piece[,] Board { get; private set; }
        public Game Game { get; private set; }
        public Grid Grid { get; private set; }
        public Grid Promote { get; set; }
        public ImageSelector Images { get; private set; }

        public int ID { get; private set; }
        public RotateTransform Ro { get; private set; }
    }

}
