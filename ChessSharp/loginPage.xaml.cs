using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessSharp
{
    /// <summary>
    /// Interaction logic for loginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        private dynamic PlayerLoginInfo;
        public static string username;


        public LoginPage()
        {

            InitializeComponent();


        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginBar.Background != Brushes.Gray)
            {
                ClearAllTextBoxes();
                DisplaySigninInfo();
            }

        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (newUserBar.Background != Brushes.Gray)
            {
                ClearAllTextBoxes();
                DisplaySignupInfo();
            }

        }


        private void Signin_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(Username_tb.Text) || string.IsNullOrEmpty(Password_tb.Password)))
            {
                CheckForSignIn();
            }
            ClearAllTextBoxes();
        }

        private void CheckForSignIn()
        {
            string username1 = Username_tb.Text.ToLower();
            ServerFunctions SV = new ServerFunctions();
            PlayerLoginInfo = SV.CheckPlayerInfo(username1, Password_tb.Password);
            if (loginBar.Background == Brushes.Gray)  //Check for Sign in 
            {
                
                if (PlayerLoginInfo["login"] == true)
                {
                    username = Username_tb.Text.ToLower();
                    ((LoginWindow)App.Current.MainWindow).ShowLobby(); //Change to lobby screen
                }
                else
                {
                    DisplayInvalidLoginMB();
                }
            }
            else   //Check for sign up
            {
                if (PlayerLoginInfo["username"] == false) //check if username is not already in database
                {

                    SV.SendPlayerInfo(username1, Password_tb.Password);
                    DisplaySigninInfo();
                }
                else
                {
                    DisplayInvalidUsernameMB();
                }
            }
        }

    

        private void DisplayInvalidLoginMB()
        {
            string msgtext = "Username/Password incorrect!";
            string txt = "Invalid";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.OK:
                    break;
            }
        }

        private void DisplayInvalidUsernameMB()
        {
            string msgtext = "Username already Taken!";
            string txt = "Invalid Username";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            switch (result)
            {
                case MessageBoxResult.OK:
                    break;
            }
        }

        private void DisplaySigninInfo()
        {
            loginBar.Background = Brushes.Gray;
            newUserBar.Background = Brushes.LightGray;
            Age_Label.Visibility = Visibility.Hidden;
            Age_tb.Visibility = Visibility.Hidden;
            Signin_Button.Content = "Sign In";
        }

        private void DisplaySignupInfo()
        {
            loginBar.Background = Brushes.LightGray;
            newUserBar.Background = Brushes.Gray;
            Age_Label.Visibility = Visibility.Visible;
            Age_tb.Visibility = Visibility.Visible;
            Signin_Button.Content = "Sign up";
        }

        private void ClearAllTextBoxes()
        {
            Age_tb.Text = "";
            Username_tb.Text = "";
            Password_tb.Password = "";
        }
    }
}
