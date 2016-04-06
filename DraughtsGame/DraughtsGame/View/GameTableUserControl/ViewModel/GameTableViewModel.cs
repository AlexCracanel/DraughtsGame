using DraughtsGame.Commands;
using DraughtsGame.Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DraughtsGame.View.GameTableUserControl.ViewModel
{

    class GameTableViewModel : BasePropertyChange
    {

        public ObservableCollection<Piece> Pices
        {
            get;
            set;
        }

        private Piece selectedPiece;

        public GameTableViewModel()
        {
            Pices = new ObservableCollection<Piece>();

            AddEmptyQueen();
            AddBlackQueen();
            AddWhiteQueen();           
        }

        private void AddBlackQueen()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    Pices.Add(new Piece(1, i, j, @"\Images\blackCheker.png"));
                }
            }
        }

        private void AddWhiteQueen()
        {
            for (int i = 5; i < 8; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    Pices.Add(new Piece(1, i, j, @"\Images\whiteChecker.png"));
                }
            }
        }

        private void AddEmptyQueen()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pices.Add(new Piece(0, i, j));
                }
            }
        }

        private ICommand moveGameCommand;
        public ICommand MoveGameCommand
        {
            get
            {
                if (moveGameCommand == null)
                {
                    
                    moveGameCommand = new RelayCommand(new Action<object>(this.Move));
                }
                return moveGameCommand;
            }
        }

        public void Move(Object parameter)
        {
            var piece = parameter as Piece;

            if (piece != null && piece.Type != 0)
            {
                selectedPiece = piece;
            }
            else
            {
                if(selectedPiece != null && piece.Type == 0)
                {
                    selectedPiece.Row = piece.Row;
                    selectedPiece.Column = piece.Column;
                }
            }
        }
    }
}
