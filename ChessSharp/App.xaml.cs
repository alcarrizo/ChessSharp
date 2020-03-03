using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

namespace ChessSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            CloseGame();
            SignOutUser();

        }

        private void SignOutUser()
        {
            string name = LoginPage.username;
            var request = (HttpWebRequest)WebRequest.Create("https://chesssharp.000webhostapp.com/userOffline.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = name
                });

                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                Console.WriteLine(result);
            }
            response.Close();
        }
        private void CloseGame()
        {
            string name = LoginPage.username;
            var request = (HttpWebRequest)WebRequest.Create("https://chesssharp.000webhostapp.com/closeGame.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = name
                });

                streamWriter.Write(json);

            }
            WebResponse response = request.GetResponse();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                Console.WriteLine(result);
            }
            response.Close();
        }
    }
}
