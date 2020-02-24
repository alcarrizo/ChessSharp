using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.MobileControls;

namespace ChessSharp
{
    class LobbytoServer
    {


        public static string GetRandomAlphaNumeric()
        {
            
            Random random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return(new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(8).ToArray()));
        }




    }
}
