using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace ChessSharp
{
    /// <summary>
    /// Interaction logic for GameLobby.xaml
    /// </summary>
    public partial class GameLobby : Window
    {
        public ObservableCollection<GameList> gameLists;
        public GameLobby()
        {
            InitializeComponent();
            NewList();
            ServerFunctions SV = new ServerFunctions();
            dynamic games = SV.RefreshLobby();

            Console.WriteLine(games.Count);
            for (int i = 0; i < games.Count; i++)
            {
                gameLists.Add(new GameList() { username = games[i].username, totalPlayers = games[i].playerCount + "/2", gameId = games[i].gameId });

            }
        }

        private void NewList()
        {
            gameLists = new ObservableCollection<GameList>();
            lbUsers.DataContext = gameLists;
        }

        private bool gameCreated = false;

        private void GameButton_Click_1(object sender, RoutedEventArgs e)
        {
            ShowMessageBox_Click(sender, e);

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            string name = LoginPage.username;
            string msgtext = "Are you sure you want to logout," + name + " ?";
            string txt = "Logout Window";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.CloseAllWindows();
                    LoginWindow mw = new LoginWindow();
                    mw.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }



        public class GameList
        {
            public string username { get; set; }
            public string totalPlayers { get; set; }
            public string gameId { get; set; }

        }

        


        private void ShowMessageBox_Click(object sender, RoutedEventArgs e)
        {
            gameCreated = false;
            string msgtext = "Are you sure you want to create a game?";
            string txt = "Create Game Window";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    gameCreated = true;
                    CreateGame();
                    ChessGame cg = new ChessGame();
                    cg.Show();

                    break;
                case MessageBoxResult.No:
                    break;
            }


        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Creates a button and grabs the row index starting at 0
            var btn = sender as Button;
            var item = btn.CommandParameter as GameList;
            var index = gameLists.IndexOf(item);

            string Id = gameLists[index].gameId;
            
            
            string msgtext = "Are you sure you want to join this game?";
            string txt = "Join Game Window";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    gameCreated = true;
                    JoinGame(Id);
                    ChessGame cg = new ChessGame();
                    cg.Show();

                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void JoinGame(string id)
        {

            ServerFunctions SV = new ServerFunctions();
            SV.JoinGameId(id, LoginPage.username);

        }

        private void CreateGame()
        {
            ServerFunctions SV = new ServerFunctions();
            string name = LoginPage.username;

            dynamic dynamicD = SV.SetGameID(name);
            gameLists.Add(new GameList() { username = name, totalPlayers = dynamicD["playerCount"] + "/2", gameId = dynamicD["gameId"] });


        }

        public void GameLobby_Closing(object sender, CancelEventArgs e)
        {
            ServerFunctions SV = new ServerFunctions();
            SV.SignOutUser();

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            gameLists.Clear();
            ServerFunctions SV = new ServerFunctions();
            dynamic games = SV.RefreshLobby();

            Console.WriteLine(games.Count);
            for (int i = 0; i < games.Count; i++)
            {
                gameLists.Add(new GameList() { username = games[i].username, totalPlayers = games[i].playerCount + "/2", gameId = games[i].gameId });

            }
        }
    }
}