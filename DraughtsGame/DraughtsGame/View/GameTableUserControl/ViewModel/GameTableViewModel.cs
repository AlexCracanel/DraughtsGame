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

        public GameTableViewModel() {}

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

        private ICommand saveGameCommand;
        public ICommand SaveGameCommand
        {
            get
            {
                if (saveGameCommand == null)
                {
                    saveGameCommand = new RelayCommand(new Action<object>(SaveGame));
                }
                return saveGameCommand;
            }
        }

        private ICommand loadGameCommand;
        public ICommand LoadGameCommand
        {
            get
            {
                if (loadGameCommand == null)
                {
                    loadGameCommand = new RelayCommand(new Action<object>(LoadGame)); 
                }
                return loadGameCommand;
            }
        }
    }
}
