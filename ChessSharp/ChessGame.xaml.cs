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

            board = new GameBoard();
            view = new GameView(board, cBoard);



            view.Update();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

            DrawGameArea();




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
                    Fill = nextIsOdd ? Brushes.White : Brushes.Gray
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


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //int tempx;
            //int tempy = Grid.GetColumn(e.GetPosition);

            //if(e.Source == Image.UidProperty)

            Image g = (Image)e.Source;
            /* if(board.CheckNull(Grid.GetColumn(g),Grid.GetRow(g)))
             {

             }*/
            if (clicked == false && !board.CheckNull(Grid.GetColumn(g), Grid.GetRow(g)))
            {
                clicked = true;
                startX = Grid.GetColumn(g);
                startY = Grid.GetRow(g);

            }
            else if (clicked)
            {
                clicked = false;
                endX = Grid.GetColumn(g);
                endY = Grid.GetRow(g);

                if (startX != endX || startY != endY)
                {
                    if (!board.AllyPieces(startX, startY, endX, endY))
                    {
                        board.Move(startX, startY, endX, endY);
                    }
                }


                view.Update();


            }

        }
    }
}
