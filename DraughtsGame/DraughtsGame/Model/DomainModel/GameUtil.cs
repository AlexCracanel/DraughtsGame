using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Model.DomainModel
{
    public class GameUtil
    {
        public enum CellState
        {
            UNITIALIZED,
            RED,
            BLACK,
            RED_KING,
            BLACK_KING
        }

        public enum GameState
        {
            UNITIALIZED,
            IN_GAME,
            FINISHED
        }

        public enum CurrentPlayer
        {
            RED_PLAYER,
            BLACK_PLAYER
        }
    }
}
