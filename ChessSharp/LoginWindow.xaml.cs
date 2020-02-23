using ChessSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
       public static bool testing = true;

        public LoginWindow()
        {

            ServerOnlineCheck();

        }

        private void ServerOnlineCheck()
        {
            InitializeComponent();
            if (testing)
            {
                var bypassButton = new Button { Content = "bypass" };

                //add event handler 1
                bypassButton.Click += ClickHandler1;
                sp.Children.Add(bypassButton);

            }
            else
            {
                WebRequest request = WebRequest.Create("http://localhost/index.php");
                request.Credentials = CredentialCache.DefaultCredentials;
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        InitializeComponent();//Load Webpage
                    }
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        string msgtext = "Chess# Server is not running!";
                        string txt = "Server Down";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, msgtext, txt, button);
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                break;
                        }
                        Application.Current.Shutdown();
                    }
                }
            }
        }


        private void ClickHandler1(object sender, RoutedEventArgs e)
        {

            ((LoginWindow)App.Current.MainWindow).ShowLobby(); //Change to lobby screen
        }

        public void ShowLobby()
        {

            GameLobby gl = new GameLobby();
            gl.Show();
            this.Close();
        }
    }
}
