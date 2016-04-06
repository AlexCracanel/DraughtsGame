using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Model.DomainModel
{
    public class GameUtil
    {

        public enum PieceType
        {
            EMPTY,
            WHITE_QUEEN,
            BLACK_QUEEN,
            WHITE_KING,
            BLACK_KING 
        }

        public enum CurrentPlayer
        {
            WHITE_PLAYER,
            BLACK_PLAYER
        }
    }
}
