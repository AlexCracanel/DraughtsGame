using DraughtsGame.Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Model.ServiceLayer
{
    class GameServices
    {
        GameTable gameTable;

        GameUtil.GameState gameState;
        GameUtil.CurrentPlayer currentPlayer;

        GameServices()
        {
            gameTable = new GameTable();
            gameState = GameUtil.GameState.UNITIALIZED;
            currentPlayer = GameUtil.CurrentPlayer.RED_PLAYER;
        }

        public bool Move(Tuple<int,int> startPosition, Tuple<int,int> endPosition)
        {
            return false;
        } 

        public void RestartGame()
        {
            currentPlayer = GameUtil.CurrentPlayer.RED_PLAYER;
            gameTable.RestartGameTable();
        }

        public bool CanPlayerRedTake() 
        {
           

            return false;
        }

        public bool CanRedQueenAtPositionJump() 
        {
            

            return false;
        }

    }
}
