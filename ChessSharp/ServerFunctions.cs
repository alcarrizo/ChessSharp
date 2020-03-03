using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ChessSharp
{
    class ServerFunctions
    {
        private string host = "https://chesssharp.000webhostapp.com";


        public void SendPlayerInfo(string userN, string passW)
        {
            //https:// stackoverflow.com/questions/9145667/how-to-post-json-to-a-server-using-c
            //https:// docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class

            
            var request = (HttpWebRequest)WebRequest.Create( host + "/createUser.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = userN,
                    password = passW
                });

                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            response.Close();

        }

        public dynamic CheckPlayerInfo(string userN, string passW)
        {
            dynamic dynamic;
            var request = (HttpWebRequest)WebRequest.Create( host + "/loginUser.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = userN,
                    password = passW
                });

                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                dynamic = jsonStr;
            }
            response.Close();
            return dynamic;
        }


        public dynamic SetGameID(string name)
        {
            dynamic temp;
            var request = (HttpWebRequest)WebRequest.Create(  host + "/createGameId.php");
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
                dynamic jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                temp = jsonStr;
            }
            response.Close();

            return temp;

        }

        public void SignOutUser()
        {
            string name = LoginPage.username;
            var request = (HttpWebRequest)WebRequest.Create( host + "/userOffline.php");
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

        public void CloseGame()
        {
            string name = LoginPage.username;
            var request = (HttpWebRequest)WebRequest.Create( host + "/closeGame.php");
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
