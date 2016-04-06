using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DraughtsGame.Commands;
using DraughtsGame.Model.DomainModel;
using System.Collections.ObjectModel;

namespace DraughtsGame.Model.ServiceLayer
{
    class GameActions
    {
        public ObservableCollection<Piece> Pices
        {
            get;
            set;
        }

        private Piece selectedPiece;

        private GameUtil.CurrentPlayer currentPlayer;

        public GameActions()
        {
            Pices = new ObservableCollection<Piece>();

            AddEmptyQueen();
            AddBlackQueen();
            AddWhiteQueen();

            currentPlayer = GameUtil.CurrentPlayer.BLACK_PLAYER;
        }

        private void AddBlackQueen()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    Pices.Add(new Piece(GameUtil.PieceType.BLACK_QUEEN, i, j, @"\Images\blackCheker.png"));
                }
            }
        }

        private void AddWhiteQueen()
        {
            for (int i = 5; i < 8; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    Pices.Add(new Piece(GameUtil.PieceType.WHITE_QUEEN, i, j, @"\Images\whiteChecker.png"));
                }
            }
        }

        private void AddEmptyQueen()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pices.Add(new Piece(GameUtil.PieceType.EMPTY, i, j));
                }
            }
        }

        public void Move(Object parameter)
        {
            var piece = parameter as Piece;

            if (piece != null && piece.Type != GameUtil.PieceType.EMPTY)
            {
                selectedPiece = piece;
            }
            else
            {
                if (selectedPiece != null && piece.Type == GameUtil.PieceType.EMPTY)
                {
                    switch (selectedPiece.Type)
                    {
                        case GameUtil.PieceType.BLACK_QUEEN:
                            if (currentPlayer == GameUtil.CurrentPlayer.BLACK_PLAYER)
                            {
                                if (MoveForBlackQueen(piece))
                                {
                                    currentPlayer = GameUtil.CurrentPlayer.WHITE_PLAYER;
                                }
                            }
                            break;

                        case GameUtil.PieceType.WHITE_QUEEN:

                            if (currentPlayer == GameUtil.CurrentPlayer.WHITE_PLAYER)
                            {
                                if (MoveForWhiteQueen(piece))
                                {
                                    currentPlayer = GameUtil.CurrentPlayer.BLACK_PLAYER;
                                }
                            }
                            break;

                        default: break;
                    }
                }
            }
        }

        private bool MoveForBlackQueen(Piece futurePositon)
        {
            if (WhitePieceNeighbor() == null)
            {
                if (IsValidBlackMove(futurePositon))
                {
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    return true;
                }
            }
            else
            {
                if (IsValidBlackTake(futurePositon))
                {
                    Piece winPiece = WhitePieceNeighbor();
                    Pices.Remove(winPiece);
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    return true;
                }
            }

            return false;
        }

        private Piece WhitePieceNeighbor()
        {
            foreach (var piece in Pices)
            {
                if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                {
                    if (piece.Row == (selectedPiece.Row + 1))
                    {
                        if (piece.Type == GameUtil.PieceType.WHITE_QUEEN)
                        {
                            return piece;
                        }
                    }
                }
            }

            return null;
        }

        private bool IsValidBlackMove(Piece futurePositon)
        {
            if (futurePositon.Column != (selectedPiece.Column + 1) && futurePositon.Column != (selectedPiece.Column - 1))
            {
                return false;
            }
            else if (futurePositon.Row != (selectedPiece.Row + 1))
            {
                return false;
            }

            return true;
        }

        private bool IsValidBlackTake(Piece futurePositon)
        {
            if (futurePositon.Column != (selectedPiece.Column + 2) && futurePositon.Column != (selectedPiece.Column - 2))
            {
                return false;
            }
            else if (futurePositon.Row != (selectedPiece.Row + 2))
            {
                return false;
            }

            return true;
        }

        private bool MoveForWhiteQueen(Piece futurePositon)
        {
            if (BlackPieceNeighbor() == null)
            {
                if (IsValidWhiteMove(futurePositon))
                {
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    return true;
                }
            }
            else
            {
                if (IsValidWhiteTake(futurePositon))
                {
                    Piece winPiece = BlackPieceNeighbor();
                    Pices.Remove(winPiece);
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    return true;
                }
            }

            return false;
        }

        private Piece BlackPieceNeighbor()
        {
            foreach (var piece in Pices)
            {
                if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                {
                    if (piece.Row == (selectedPiece.Row - 1))
                    {
                        if (piece.Type == GameUtil.PieceType.BLACK_QUEEN)
                        {
                            return piece;
                        }
                    }
                }
            }

            return null;
        }

        private bool IsValidWhiteMove(Piece futurePositon)
        {
            if (futurePositon.Column != (selectedPiece.Column + 1) && futurePositon.Column != (selectedPiece.Column - 1))
            {
                return false;
            }
            else if (futurePositon.Row != (selectedPiece.Row - 1))
            {
                return false;
            }

            return true;
        }

        private bool IsValidWhiteTake(Piece futurePositon)
        {
            if (futurePositon.Column != (selectedPiece.Column + 2) && futurePositon.Column != (selectedPiece.Column - 2))
            {
                return false;
            }
            else if (futurePositon.Row != (selectedPiece.Row - 2))
            {
                return false;
            }

            return true;
        }
    }
}
