using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessSharp
{
    public enum Type {PAWN,KNIGHT,QUEEN,KING,BISHOP,ROOK,EMPTY}

    interface IPiece
    {

        /// <summary>
        /// Function to check the move was valid
        /// </summary>
        /// <returns>true or false </returns>
        bool ValidMove(int startX, int startY, int endX, int endY, IPiece[,] Board);
        bool ValidPath(int startX, int startY, int endX, int endY, IPiece[,] Board);


        /// <summary>
        /// function to get the piece type
        /// </summary>
        /// <returns>the name of the piece(i.e. pawn, rook, king, queen, etc...)</returns>
        Type GetType();

        bool GetColor();

        string GetName();
    }
}
