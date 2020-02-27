using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace ChessSharp
{
    class LobbytoServer
    {
        private string localgameId;

        public static string GetRandomAlphaNumeric()
        {
            
            Random random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return(new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(8).ToArray()));
            
        }



        public void SendGameID(string name)
        {


            localgameId = GetRandomAlphaNumeric();

            var request = (HttpWebRequest)WebRequest.Create("http://localhost/serverCode/createGameId.php");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    username = name,
                    gameId = localgameId
                });

                streamWriter.Write(json);
            }

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            /*using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic jsonStr = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                string temp = jsonStr;
            }
            */
            response.Close();


        }




    }
}
