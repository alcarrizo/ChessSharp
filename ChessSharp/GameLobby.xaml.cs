using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            newList();
            
        }

        private void newList()
        {
            gameLists = new ObservableCollection<GameList>();
            lbUsers.DataContext = gameLists;
        }

        private bool gameCreated = false;

        private void GameButton_Click_1(object sender, RoutedEventArgs e)
        {
            ShowMessageBox_Click(sender, e);
            if (gameCreated == true)
            {
                gameLists.Add(new GameList() {username="test2", totalPlayers = "" + 1 + "/2" });
            }
            

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            string msgtext = "Are you sure you want to logout?";
            string txt = "Logout Window";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                   // MainWindow mw = new MainWindow();
                   // mw.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        
        
        public class GameList
        {
            public string username { get; set; }
            public string totalPlayers { get; set; }
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
                    ChessGame cg = new ChessGame();
                    cg.Show();
                    
                    break;
                case MessageBoxResult.No:
                    break;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            string msgtext = "Are you sure you want to join this game?";
            string txt = "Join Game Window";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    gameCreated = true;
                    ChessGame cg = new ChessGame();
                    cg.Show();

                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }
}
