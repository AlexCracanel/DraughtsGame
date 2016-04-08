using DraughtsGame.Commands;
using DraughtsGame.Model.DomainModel;
using DraughtsGame.Model.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DraughtsGame.View.GameTableUserControl.ViewModel
{

    class GameTableViewModel : GameActions
    {

        public GameTableViewModel()
        {
                 
        }

        private ICommand moveGameCommand;
        public ICommand MoveGameCommand
        {
            get
            {
                if (moveGameCommand == null)
                {
                    
                    moveGameCommand = new RelayCommand(new Action<object>(Move));
                }
                return moveGameCommand;
            }
        }

        private ICommand restartGameCommand;
        public ICommand RestartGameCommand 
        {
            get
            {
                if (restartGameCommand == null)
                {
                    //restartGameCommand = new RelayCommand(new Action<object>());
                }
                return restartGameCommand;
            }
        }
    }
}
