using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessSharp
{
    public class Movement
    {
        public Movement()
        {
            check = false;
            checkMate = false;
            forfeit = false;
            askForDraw = false;
            enPassant = false;
            castling = false;
            promotion = false;
        }
        public bool? check { get; set; }
        public bool? checkMate { get; set; }
        public bool? forfeit { get; set; }

        public bool? askForDraw { get; set; }
        public int startX { get; set; }

        public int startY { get; set; }

        public int endX { get; set; }

        public int endY { get; set; }

        public bool? enPassant { get; set; }

        public int pawnX { get; set; }

        public int pawnY { get; set; }

        public bool? castling { get; set; }

        public int rookStartX { get; set; }

        public int rookStartY { get; set; }

        public int rookEndX { get; set; }

        public int rookEndY { get; set; }

        public bool? promotion { get; set; }

        public string pawnEvolvesTo { get; set; }

        public string name { get; set; }

        public string username { get; set; }

        public string gameId { get; set; }


    }
}
