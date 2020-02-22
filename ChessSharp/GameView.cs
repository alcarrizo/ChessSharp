using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessSharp
{
    class GameView
    {
        public GameView(GameBoard board, Grid grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid is null");
            }

            if (board == null)
            {
                throw new ArgumentNullException("board is null");
            }

            Grid = grid;
            Board = board;
            Images = new ImageSelector();

        }

        public void Update()
        {
            Grid.Children.Clear();

            Image image = new Image();

            RotateTransform ro = new RotateTransform(90);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board.Board[i, j] == null)
                        image = GetImage(Board.Board[i, j], false);
                    else
                        image = GetImage(Board.Board[i, j], Board.Board[i, j].Color);
                    Grid.SetZIndex(image, 4);
                    Grid.SetColumn(image, i);
                    Grid.SetRow(image, j);

                    image.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5); // sets the rotation point of the image to the center of the image
                    image.RenderTransform = ro;         // Applies the relevant rotation

                    Grid.Children.Add(image);
                }
            }
        }

        private Image GetImage(Piece piece, bool white)
        {
            Image image = new Image();

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

        public Grid Grid { get; private set; }
        public GameBoard Board { get; private set; }
        public ImageSelector Images { get; private set; }
    }
}
