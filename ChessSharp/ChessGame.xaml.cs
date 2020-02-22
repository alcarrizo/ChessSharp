using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChessGame : Window
    {
        const int SpaceSize = 70;
        private GameBoard board;
        private GameView view;


        public ChessGame()
        {

            InitializeComponent();

            // How to change the rotation of the grid
            RotateTransform ro = new RotateTransform(-90);

            cBoard.RenderTransform = ro;
            Highlights.RenderTransform = ro;

            board = new GameBoard();
            view = new GameView(board, cBoard);

            view.Update();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

            DrawGameArea();
            SetUpHighlights();



        }

        private void SetUpHighlights()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.Transparent;
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, j);
                    Highlights.Children.Add(rect);
                }
            }
        }

        private void DrawGameArea()
        {
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;

            while (doneDrawingBackground == false)
            {
                Rectangle rect = new Rectangle
                {
                    Width = SpaceSize,
                    Height = SpaceSize,
                    Fill = nextIsOdd ? Brushes.Gray : Brushes.White
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += SpaceSize;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += SpaceSize;
                    rowCounter++;
                    nextIsOdd = (rowCounter % 2 != 0);
                }

                if (nextY >= GameArea.ActualHeight)
                    doneDrawingBackground = true;
            }
        }

        private bool clicked = false;
        private int startX;
        private int startY;
        private int endX;
        private int endY;
        private int startBlockIndex;
        private Brush brush;

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Image g = (Image)e.Source;



            if (clicked == false && !board.CheckNull(Grid.GetColumn(g), Grid.GetRow(g)))
            {
                startBlockIndex = cBoard.Children.IndexOf(g);
                clicked = true;
                startX = Grid.GetColumn(g);
                startY = Grid.GetRow(g);

                Rectangle rect = (Rectangle)Highlights.Children[startBlockIndex];

                brush = rect.Fill;

                rect.Fill = Brushes.Teal;
            }
            else if (clicked)
            {

                clicked = false;
                endX = Grid.GetColumn(g);
                endY = Grid.GetRow(g);

                Rectangle rect = (Rectangle)Highlights.Children[startBlockIndex];
                rect.Fill = brush;

                if (startX != endX || startY != endY)
                {
                    if (!board.AllyPieces(startX, startY, endX, endY))
                    {
                        board.Move(startX, startY, endX, endY, Highlights);
                    }
                }

                view.Update();


            }

        }

        private void Highlights_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)e.Source;
            rect.Fill = Brushes.Blue;
        }
    }
}
