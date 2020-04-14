using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Point = System.Drawing.Point;
using System.Web.Script.Serialization;

namespace ChessSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChessGame : Window
    {
        const int SpaceSize = 70;
        private GameBoard board;
        private Grid PromoteGrid;

        private Player p1 = new Player("Player1", true);
        private Player p2 = new Player("Player2", false);
        private Player player;
        private Player currPlay;
        private bool currColor;
        private RotateTransform ro;
        bool twoPlayer = false;
        bool oppLeftGame = false;
        bool closed = false; // used to tell if you are the person who closed the window

        // for local playing if we decide to implement that
        public ChessGame()
        {

            InitializeComponent();

            // How to change the rotation of the grid
            ro = new RotateTransform(180);
            //RotateTransform ro = new RotateTransform(0);


            //cBoard.RenderTransform = ro;
            //Highlights.RenderTransform = ro;

            board = new GameBoard(cBoard, ro);
            //view = new GameView(board, cBoard);
            GameGrid.RenderTransform = ro;
            board.Update();

            if (player.Color == true)
            {
                YourName.Background = Brushes.White;
            }

            currPlay = p1;
            DrawGameArea();
            SetUpHighlights();
        }

        public ChessGame(bool control)
        {


            InitializeComponent();

            // changes the rotation of the grid depending on what pieces you control
            if (control == true)
            {
                ro = new RotateTransform(180);
                player = new Player(LoginPage.username, control);
            }
            else
            {
                ro = new RotateTransform(0);
                player = new Player(LoginPage.username, control);
            }


            board = new GameBoard(cBoard, ro);

            GameArea.RenderTransform = ro;

            board.Update();


            currColor = true;
            DrawGameArea();
            SetUpHighlights();
            YourName.Content = LoginPage.username;
            if (player.Color == true)
            {
                YourName.Background = Brushes.White;

            }
            else
            {
                OppName.Background = Brushes.White;
            }
            GetSessionInfo();
        }


        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            ServerFunctions SV = new ServerFunctions();
            dynamic checkSession = SV.GetSessionDetails();
            bool left = false;

            if (player.Color == false)
            {
                await Task.Run(() => WaitForOpponent());
            }

            while (left == false && closed == false)
            {
                await Task.Delay(1000);
                await Task.Run(() => left = DidOpponentLeave());
            }
            if (left == true && closed == false)
            {
                oppLeftGame = true;
                await Task.Run(() =>
                   Application.Current.Dispatcher.Invoke((Action)delegate
                   {
                       EndGame();
                   })
                      );
            }








        }

        public bool DidOpponentLeave()
        {
            ServerFunctions SV = new ServerFunctions();
            dynamic checkSession = SV.GetSessionDetails();

            if (checkSession != null)
            {

                return false;

            }
            else
            {
                return true;
            }
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
                if (nextX >= GameArea.Width)
                {
                    nextX = 0;
                    nextY += SpaceSize;
                    rowCounter++;
                    nextIsOdd = (rowCounter % 2 != 0);
                }

                if (nextY >= GameArea.Height)
                    doneDrawingBackground = true;
            }
        }

        private bool clicked = false;
        private Point start;
        private Point end;
        private int startBlockIndex;
        private Brush brush;
        Movement moveInfo;

        private async void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Image g = (Image)e.Source;
            moveInfo = new Movement();



            if (twoPlayer == true && clicked == false && !board.CheckNull(Grid.GetColumn(g), Grid.GetRow(g))
                && board.ControlPiece(new Point(Grid.GetColumn(g), Grid.GetRow(g)), player)
                && (player.Color == currColor))
            {
                startBlockIndex = cBoard.Children.IndexOf(g);
                clicked = true;
                start.X = Grid.GetColumn(g);
                start.Y = Grid.GetRow(g);

                Rectangle rect = (Rectangle)Highlights.Children[startBlockIndex];

                brush = rect.Fill;

                rect.Fill = Brushes.Teal;
            }
            else if (clicked)
            {
                moveInfo.username = LoginPage.username;
                moveInfo.gameId = GameLobby.gameId;
                clicked = false;
                end.X = Grid.GetColumn(g);
                end.Y = Grid.GetRow(g);

                Rectangle rect = (Rectangle)Highlights.Children[startBlockIndex];
                rect.Fill = brush;

                if (start.X != end.X || start.Y != end.Y)
                {
                    bool move = false;
                    move = board.Move(start, end, Highlights, currPlay, moveInfo);

                    if (move == true)
                    {
                        moveInfo.startX = start.X;
                        moveInfo.startY = start.Y;
                        moveInfo.endX = end.X;
                        moveInfo.endY = end.Y;

                        if (board.IsPawn(end) && (end.Y == 7 || end.Y == 0))
                        {
                            moveInfo.promotion = true;
                            PromoteGrid = CreatePromoteGrid();
                            PromoteGrid.RenderTransform = FullScreen.RenderTransform;
                            PromoteGrid.RenderTransformOrigin = FullScreen.RenderTransformOrigin;
                            FullScreen.Children.Add(PromoteGrid);
                            cBoard.IsHitTestVisible = false;
                        }
                        else
                        {
                            if (board.EnemyChecks(end, Highlights, moveInfo))
                            {
                                SendMessage(moveInfo);
                                EndGame();

                            }
                            else
                            {


                                currColor = !currColor;
                                SendMessage(moveInfo);
                                await Task.Run(() => WaitForOpponent());

                            }

                            YourName.Background = Brushes.Transparent;
                            OppName.Background = Brushes.White;
                            board.Update();


                        }
                    }

                }
            }
        }

        // brings up the end game options
        private void EndGame()
        {
            ServerFunctions SV = new ServerFunctions();
            MessageBoxResult result = MessageBoxResult.Cancel;

            if (oppLeftGame == true)
            {
                if (closed == true)
                {
                    this.Close();
                }
                else
                {
                    result = MessageBox.Show("Opponent has left the game");

                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            {

                                if (oppLeftGame == true)
                                {
                                    this.Close();
                                }

                                break;
                            }
                    }
                }
            }
            else
            {
                if (moveInfo.checkMate == true)
                {
                    result = MessageBox.Show("Checkmate");
                }
                else if (moveInfo.forfeit == true)
                {
                    result = MessageBox.Show(moveInfo.username + " Forfeits the Match");

                }
                switch (result)
                {
                    case MessageBoxResult.OK:
                        {


                            currColor = true;
                            board = new GameBoard(cBoard, ro);
                            ResetHighlights();
                            board.Update();
                            newGame = true;

                            if (player.Color == true)
                            {
                                YourName.Background = Brushes.White;
                            }
                            else
                            {
                                OppName.Background = Brushes.Transparent;
                                WaitForOpponent();
                            }
                        }
                        break;
                }
            }
        }

        private void ResetHighlights()
        {
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Rectangle rect = (Rectangle)Highlights.Children[index];
                    if (rect.Fill == Brushes.Red)
                    {
                        rect.Fill = Brushes.Transparent;
                    }
                    index++;
                }
            }
        }

        private Grid CreatePromoteGrid()
        {
            Grid temp = new Grid();
            temp.Background = Brushes.Transparent;
            temp.VerticalAlignment = VerticalAlignment.Center;
            temp.HorizontalAlignment = HorizontalAlignment.Center;
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(75);
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(70);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(70);
            RowDefinition row3 = new RowDefinition();
            row3.Height = new GridLength(70);
            RowDefinition row4 = new RowDefinition();
            row4.Height = new GridLength(70);
            temp.ShowGridLines = true;
            temp.RowDefinitions.Add(row1);
            temp.RowDefinitions.Add(row2);
            temp.RowDefinitions.Add(row3);
            temp.RowDefinitions.Add(row4);
            temp.ColumnDefinitions.Add(col1);
            Panel.SetZIndex(temp, 10);


            Button b1 = new Button();
            b1.Content = "Queen";
            b1.HorizontalAlignment = HorizontalAlignment.Center;
            b1.Width = 70;
            b1.Click += Promotion;

            Button b2 = new Button();
            b2.Content = "Knight";
            b2.HorizontalAlignment = HorizontalAlignment.Center;
            b2.Width = 70;
            Grid.SetRow(b2, 1);
            b2.Click += Promotion;

            Button b3 = new Button();
            b3.Content = "Rook";
            b3.HorizontalAlignment = HorizontalAlignment.Center;
            b3.Width = 70;
            Grid.SetRow(b3, 2);
            b3.Click += Promotion;

            Button b4 = new Button();
            b4.Content = "Bishop";
            b4.HorizontalAlignment = HorizontalAlignment.Center;
            b4.Width = 70;
            Grid.SetRow(b4, 3);
            b4.Click += Promotion;

            temp.Children.Add(b1);
            temp.Children.Add(b2);
            temp.Children.Add(b3);
            temp.Children.Add(b4);

            return temp;
        }


        private async void Promotion(object sender, RoutedEventArgs e)
        {


            Button temp = (Button)e.Source;
            string name = (string)temp.Content;

            moveInfo.pawnEvolvesTo = name;

            board.ChangePiece(end, name);
            if (board.EnemyChecks(end, Highlights, moveInfo))
            {
                SendMessage(moveInfo);
                EndGame();

            }


            board.Update();
            FullScreen.Children.Remove(PromoteGrid);
            cBoard.IsHitTestVisible = true;

            SendMessage(moveInfo);
            YourName.Background = Brushes.Transparent;
            OppName.Background = Brushes.White;
            currColor = !currColor;
            await Task.Run(() => WaitForOpponent());

        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            closed = true;
            ServerFunctions SV = new ServerFunctions();
            SV.CloseGame(GameLobby.gameId);
            GameLobby gl = new GameLobby();
            gl.Show();

        }


        private void SendMessage(Movement moveInfo)
        {
            ServerFunctions SV = new ServerFunctions();

            // converts object into a json message
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(moveInfo);
            SV.SendMove(result);
            //SV.GetMove();
            //converts json message back into class
            //Movement temp = Newtonsoft.Json.JsonConvert.DeserializeObject<Movement>(result);

        }

        private async void GetSessionInfo()
        {
            ServerFunctions SV = new ServerFunctions();
            dynamic getInfo = null;
            while (getInfo == null || getInfo["playerCount"] == 1)
            {
                await Task.Delay(750);
                getInfo = SV.GetSessionDetails();
            }

            if (player.Color == true)
            {
                OppName.Content = getInfo["joinedUser"];
            }
            else
            {
                OppName.Content = getInfo["username"];
            }
            twoPlayer = true;
        }



        bool newGame = false;

        // function to make player wait for the server to send the information from the opponents move
        //private void WaitForOpponent()
        public async void WaitForOpponent()
        {
            ServerFunctions SV = new ServerFunctions();
            dynamic getMove = null;
            dynamic checkSession = null;



            moveInfo = new Movement();



            while (getMove == null || getMove["lastMove"] == LoginPage.username || (getMove["checkMate"] == 1 && newGame == true)
                || (getMove["forfeit"] == 1 && newGame == true))
            {
                await Task.Delay(750);

                checkSession = null;
                checkSession = SV.GetSessionDetails();
                /*if (checkSession == null && closed == false)
                {
                    oppLeftGame = true;
                    break;

                }*/

                getMove = SV.GetMove();


            }
            /*if (checkSession == null && closed == false)
            {
                await Task.Run(() =>
                   Application.Current.Dispatcher.Invoke((Action)delegate
                   {
                       EndGame();
                   })
                      );
            }*/


            if (newGame == true)
            {
                newGame = false;
            }

            if (getMove["check"] == 1)
            {
                moveInfo.check = true;
            }
            else
            {
                moveInfo.check = false;
            }

            if (getMove["checkMate"] == 1)
            {
                moveInfo.checkMate = true;
            }
            else
            {
                moveInfo.checkMate = false;
            }

            if (getMove["enPassant"] == 1)
            {
                moveInfo.enPassant = true;
            }
            else
            {
                moveInfo.enPassant = false;
            }

            if (getMove["promotion"] == 1)
            {
                moveInfo.promotion = true;
            }
            else
            {
                moveInfo.promotion = false;
            }

            if (getMove["castling"] == 1)
            {
                moveInfo.castling = true;
            }
            else
            {
                moveInfo.castling = false;
            }

            if (getMove["forfeit"] == 1)
            {
                moveInfo.forfeit = true;
            }
            else
            {
                moveInfo.forfeit = false;
            }
            if (getMove["askForDraw"] == 1)
            {
                moveInfo.askForDraw = true;
            }
            else
            {
                moveInfo.askForDraw = false;
            }

            moveInfo.endX = getMove["endX"];
            moveInfo.endY = getMove["endY"];
            moveInfo.startX = getMove["startX"];
            moveInfo.startY = getMove["startY"];
            moveInfo.rookStartX = getMove["rookStartX"];
            moveInfo.rookStartY = getMove["rookStartY"];
            moveInfo.rookEndX = getMove["rookEndX"];
            moveInfo.rookEndY = getMove["rookEndY"];
            moveInfo.pawnX = getMove["pawnX"];
            moveInfo.pawnY = getMove["pawnY"];
            moveInfo.pawnEvolvesTo = getMove["pawnEvolvesTo"];
            moveInfo.username = getMove["lastMove"];

            if (moveInfo.forfeit == true)
            {
                // temp = getMove;
                await Task.Run(() =>
               Application.Current.Dispatcher.Invoke((Action)delegate
               {
                   EndGame();
               })
                  );
            }

            else
            {
                board.UpdateEnemyPieces(moveInfo, Highlights);

                currColor = !currColor;

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    board.Update();
                });


                if (moveInfo.checkMate == true)
                {
                    // temp = getMove;
                    await Task.Run(() =>
                   Application.Current.Dispatcher.Invoke((Action)delegate
                     {
                         EndGame();
                     })
                      );
                }

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    YourName.Background = Brushes.White;
                    OppName.Background = Brushes.Transparent;
                });
            }

            //while ()

            /*            'ifcheck' => $row["ifcheck"], 
						'checkMate' => $row["checkMate"],
						'forfeit' => $row["forfeit"],
						'askForDraw' => $row["askForDraw"],
						'startX' => $row["startX"],
						'startY' => $row["startY"],
						'endX' => $row["endX"],
						'endY' => $row["endY"],
						'enPassant' => $row["enPassant"],
						'pawnX' => $row["pawnX"],
						'pawnY' => $row["pawnY"],
						'castling' => $row["castling"],
						'rookStartX' => $row["rookStartX"],
						'rookStartY' => $row["rookStartY"],
						'rookEndX' => $row["rookEndX"],
						'rookEndY' => $row["rookEndY"],
						'promotion' => $row["promotion"],
						'pawnEvolvesTo' => $row["pawnEvolvesTo"],
						'lastMove' => $row["lastMove"]
            */

        }

        private async void Draw_Button_Click(object sender, RoutedEventArgs e)
        {
            moveInfo = new Movement();

            moveInfo.askForDraw = true;
            moveInfo.username = LoginPage.username;
            moveInfo.gameId = GameLobby.gameId;

            SendMessage(moveInfo);
            await Task.Run(() => WaitForOpponent());
        }

        private async void Forfeit_Button_Click(object sender, RoutedEventArgs e)
        {
            moveInfo = new Movement();

            moveInfo.forfeit = true;
            moveInfo.username = LoginPage.username;
            moveInfo.gameId = GameLobby.gameId;

            SendMessage(moveInfo);
            EndGame();
        }
    }
}
