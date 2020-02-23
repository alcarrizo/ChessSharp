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
        private String username;


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
            username = Username_tb.Text;
            if (loginBar.Background == Brushes.Gray)  //Check for Sign in 
            {
                CheckPlayerInfo();
                if (PlayerLoginInfo["login"] == 1)
                {
                    ((LoginWindow)App.Current.MainWindow).ShowLobby(); //Change to lobby screen
                }
                else
                {
                    DisplayInvalidLoginMB();
                }
            }
            else   //Check for sign up
            {
                CheckPlayerInfo();
                if (PlayerLoginInfo["username"] == 0) //check if username is not already in database
                {

                    SendPlayerInfo();
                    DisplaySigninInfo();
                }
                else
                {
                    DisplayInvalidUsernameMB();
                }
            }
        }

      
        private void SendPlayerInfo()
        {
            //https:// stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c
            //https:// docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class


            var request = (HttpWebRequest)WebRequest.Create("http://localhost/serverCode/createUser.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = Username_tb.Text,
                    password = Password_tb.Password
                });

                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            response.Close();

        }

        private void CheckPlayerInfo()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost/serverCode/loginUser.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = Username_tb.Text,
                    password = Password_tb.Password
                });

                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                PlayerLoginInfo = jsonStr;
            }
            response.Close();
        }

        public String getUsername()
        {
            return username;
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
