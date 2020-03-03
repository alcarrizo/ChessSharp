using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessSharp
{
    public class Player
    {
        public Player(string name,bool color)
        {
            Name = name;
            Color = color;
        }

        public string Name { get; private set; }
        public bool Color { get; private set; }
    }
}
