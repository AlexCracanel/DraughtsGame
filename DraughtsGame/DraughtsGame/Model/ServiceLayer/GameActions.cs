using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DraughtsGame.Commands;
using DraughtsGame.Model.DomainModel;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DraughtsGame.Model.ServiceLayer
{
    class GameActions : DependencyObject
    {


        public int BlackScore
        {
            get { return (int)GetValue(BlackScoreProperty); }
            set { SetValue(BlackScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlackScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlackScoreProperty =
            DependencyProperty.Register("BlackScore", typeof(int), typeof(GameActions), new PropertyMetadata(0));

        public int WhiteScore
        {
            get { return (int)GetValue(WhiteScoreProperty); }
            set { SetValue(WhiteScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WhiteScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WhiteScoreProperty =
            DependencyProperty.Register("WhiteScore", typeof(int), typeof(GameActions), new PropertyMetadata(0));

        public ObservableCollection<Piece> Pieces
        {
            get;
            set;
        }

        private Piece selectedPiece;

        private GameUtil.CurrentPlayer currentPlayer;

        public GameActions()
        {
            Pieces = new ObservableCollection<Piece>();

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
                    Pieces.Add(new Piece(GameUtil.PieceType.BLACK_QUEEN, i, j, @"\Images\blackCheker.png"));
                }
            }
        }

        private void AddWhiteQueen()
        {
            for (int i = 5; i < 8; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < 8; j += 2)
                {
                    Pieces.Add(new Piece(GameUtil.PieceType.WHITE_QUEEN, i, j, @"\Images\whiteChecker.png"));
                }
            }
        }

        private void AddEmptyQueen()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pieces.Add(new Piece(GameUtil.PieceType.EMPTY, i, j));
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
                                MoveForBlackQueen(piece);
                            }
                            break;

                        case GameUtil.PieceType.WHITE_QUEEN:

                            if (currentPlayer == GameUtil.CurrentPlayer.WHITE_PLAYER)
                            {
                                MoveForWhiteQueen(piece);
                            }

                            break;

                        case GameUtil.PieceType.BLACK_KING:

                            if (currentPlayer == GameUtil.CurrentPlayer.BLACK_PLAYER)
                            {
                                MoveForBlackQueen(piece);
                            }

                            break;

                        case GameUtil.PieceType.WHITE_KING:

                            if (currentPlayer == GameUtil.CurrentPlayer.WHITE_PLAYER)
                            {
                                MoveForWhiteQueen(piece);
                            }

                            break;

                        default: break;
                    }

                    TransformKings();
                }
            }
        }

        #region BlackQueen

        private void MoveForBlackQueen(Piece futurePositon)
        {
            if (!BlackShoodTake() && !BlackKingShoodTake())
            {
                if (IsValidBlackMove(futurePositon) || IsValidKingMove(futurePositon))
                {
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    currentPlayer = GameUtil.CurrentPlayer.WHITE_PLAYER;

                    return;
                }
            }
            else
            {
                if (BlackCanTake(futurePositon) != null)
                {
                    Piece winPiece = BlackCanTake(futurePositon);
                    Pieces.Remove(winPiece);
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    BlackScore++;
                }
                else if (BlackKingCanTake(futurePositon) != null)
                {
                    Piece winPiece = BlackKingCanTake(futurePositon);
                    Pieces.Remove(winPiece);
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;
                    BlackScore++;
                }

                if (!BlackShoodTake() && !BlackKingShoodTake())
                {
                    currentPlayer = GameUtil.CurrentPlayer.WHITE_PLAYER;
                }
            }

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

        private Piece BlackCanTake(Piece futurePosition)
        {
            if (futurePosition.Row == selectedPiece.Row + 2)
            {
                if ((futurePosition.Column == selectedPiece.Column - 2) || (futurePosition.Column == selectedPiece.Column + 2))
                {
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                        {
                            if (piece.Row == (selectedPiece.Row + 1))
                            {
                                if (piece.Type == GameUtil.PieceType.WHITE_QUEEN || piece.Type == GameUtil.PieceType.WHITE_KING)
                                {
                                    if (piece.Column + 1 == futurePosition.Column || piece.Column - 1 == futurePosition.Column)
                                    {
                                        return piece;
                                    }
                                }
                            }

                        }
                    }
                }

            }

            return null;
        }

        private bool BlackShoodTake()
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Piece currentPiece = Pieces.ElementAt(i);

                if (currentPiece.Type == GameUtil.PieceType.BLACK_QUEEN)
                {
                    for (int j = 0; j < Pieces.Count; j++)
                    {
                        Piece pieceToVerify = Pieces.ElementAt(j);
                        int sign = 0;

                        if (pieceToVerify.Type == GameUtil.PieceType.WHITE_QUEEN || pieceToVerify.Type == GameUtil.PieceType.WHITE_KING)
                        {
                            if (pieceToVerify.Row == currentPiece.Row + 1)
                            {
                                if (pieceToVerify.Column == currentPiece.Column + 1)
                                {
                                    sign = 1;
                                }
                                else if (pieceToVerify.Column == currentPiece.Column - 1)
                                {
                                    sign = -1;
                                }

                                if (sign != 0)
                                {
                                    for (int k = 0; k < Pieces.Count; k++)
                                    {
                                        Piece secondPieceToVerify = Pieces.ElementAt(k);
                                        if (secondPieceToVerify.Type == GameUtil.PieceType.EMPTY)
                                        {
                                            if (secondPieceToVerify.Row == currentPiece.Row + 2)
                                            {
                                                if (pieceToVerify.Column + sign == secondPieceToVerify.Column)
                                                {
                                                    if (!FoundPieceAtPosition(secondPieceToVerify.Row, secondPieceToVerify.Column))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region WhiteQueen
        private void MoveForWhiteQueen(Piece futurePositon)
        {
            if (!WhiteShoodTake() && !WhiteKingShoodTake())
            {
                if (IsValidWhiteMove(futurePositon) || IsValidKingMove(futurePositon))
                {
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;

                    currentPlayer = GameUtil.CurrentPlayer.BLACK_PLAYER;

                    return;
                }
            }
            else
            {
                if (WhiteCanTake(futurePositon) != null)
                {
                    Piece winPiece = WhiteCanTake(futurePositon);
                    Pieces.Remove(winPiece);
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;
                    WhiteScore++;
                }
                else if (WhiteKingCanTake(futurePositon) != null)
                {
                    Piece winPiece = WhiteKingCanTake(futurePositon);
                    Pieces.Remove(winPiece);
                    selectedPiece.Column = futurePositon.Column;
                    selectedPiece.Row = futurePositon.Row;
                    WhiteScore++;
                }

                if (!WhiteShoodTake() && !WhiteKingShoodTake())
                {
                    currentPlayer = GameUtil.CurrentPlayer.BLACK_PLAYER;
                }
            }
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

        private Piece WhiteCanTake(Piece futurePosition)
        {
            if (futurePosition.Row == selectedPiece.Row - 2)
            {
                if ((futurePosition.Column == selectedPiece.Column - 2) || (futurePosition.Column == selectedPiece.Column + 2))
                {
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                        {
                            if (piece.Row == (selectedPiece.Row - 1))
                            {
                                if (piece.Type == GameUtil.PieceType.BLACK_QUEEN || piece.Type == GameUtil.PieceType.BLACK_KING)
                                {
                                    if (piece.Column + 1 == futurePosition.Column || piece.Column - 1 == futurePosition.Column)
                                    {
                                        return piece;
                                    }
                                }
                            }

                        }
                    }
                }

            }

            return null;
        }

        private bool WhiteShoodTake()
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Piece currentPiece = Pieces.ElementAt(i);

                if (currentPiece.Type == GameUtil.PieceType.WHITE_QUEEN)
                {
                    for (int j = 0; j < Pieces.Count; j++)
                    {
                        Piece pieceToVerify = Pieces.ElementAt(j);
                        int sign = 0;

                        if (pieceToVerify.Type == GameUtil.PieceType.BLACK_QUEEN || pieceToVerify.Type == GameUtil.PieceType.BLACK_KING)
                        {
                            if (pieceToVerify.Row == currentPiece.Row - 1)
                            {
                                if (pieceToVerify.Column == currentPiece.Column + 1)
                                {
                                    sign = 1;
                                }
                                else if (pieceToVerify.Column == currentPiece.Column - 1)
                                {
                                    sign = -1;
                                }

                                if (sign != 0)
                                {
                                    for (int k = 0; k < Pieces.Count; k++)
                                    {
                                        Piece secondPieceToVerify = Pieces.ElementAt(k);
                                        if (secondPieceToVerify.Type == GameUtil.PieceType.EMPTY)
                                        {
                                            if (secondPieceToVerify.Row == currentPiece.Row - 2)
                                            {
                                                if (pieceToVerify.Column + sign == secondPieceToVerify.Column)
                                                {
                                                    if (!FoundPieceAtPosition(secondPieceToVerify.Row, secondPieceToVerify.Column))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region BlackKing
        private bool IsValidKingMove(Piece futurePositon)  
        {
            if(selectedPiece.Type != GameUtil.PieceType.BLACK_KING && selectedPiece.Type != GameUtil.PieceType.WHITE_KING)
            {
                return false;
            }

            if (futurePositon.Column != (selectedPiece.Column + 1) && futurePositon.Column != (selectedPiece.Column - 1))
            {
                return false;
            }
            else if (futurePositon.Row != (selectedPiece.Row + 1) && futurePositon.Row != (selectedPiece.Row - 1))
            {
                return false;
            }

            return true;
        }

        private Piece BlackKingCanTake(Piece futurePosition) 
        {
            if (futurePosition.Row == selectedPiece.Row + 2)
            {
                if ((futurePosition.Column == selectedPiece.Column - 2) || (futurePosition.Column == selectedPiece.Column + 2))
                {
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                        {
                            if (piece.Row == (selectedPiece.Row + 1))
                            {
                                if (piece.Type == GameUtil.PieceType.WHITE_QUEEN || piece.Type == GameUtil.PieceType.WHITE_KING)
                                {
                                    if (piece.Column + 1 == futurePosition.Column || piece.Column - 1 == futurePosition.Column)
                                    {
                                        return piece;
                                    }
                                }
                            }

                        }
                    }
                }

            }

            if (futurePosition.Row == selectedPiece.Row - 2)
            {
                if ((futurePosition.Column == selectedPiece.Column - 2) || (futurePosition.Column == selectedPiece.Column + 2))
                {
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                        {
                            if (piece.Row == (selectedPiece.Row - 1))
                            {
                                if (piece.Type == GameUtil.PieceType.WHITE_QUEEN || piece.Type == GameUtil.PieceType.WHITE_KING)
                                {
                                    if (piece.Column + 1 == futurePosition.Column || piece.Column - 1 == futurePosition.Column)
                                    {
                                        return piece;
                                    }
                                }
                            }

                        }
                    }
                }

            }

            return null;
        }

        private bool BlackKingShoodTake() 
        {
           

            return BlackKingShoodTakeInFront() || BlackKingShoodTakeInBack();
        }

        private bool BlackKingShoodTakeInFront()
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Piece currentPiece = Pieces.ElementAt(i);

                if (currentPiece.Type == GameUtil.PieceType.BLACK_KING)
                {
                    for (int j = 0; j < Pieces.Count; j++)
                    {
                        Piece pieceToVerify = Pieces.ElementAt(j);
                        int sign = 0;

                        if (pieceToVerify.Type == GameUtil.PieceType.WHITE_QUEEN || pieceToVerify.Type == GameUtil.PieceType.WHITE_KING)
                        {
                            if (pieceToVerify.Row == currentPiece.Row + 1)
                            {
                                if (pieceToVerify.Column == currentPiece.Column + 1)
                                {
                                    sign = 1;
                                }
                                else if (pieceToVerify.Column == currentPiece.Column - 1)
                                {
                                    sign = -1;
                                }

                                if (sign != 0)
                                {
                                    for (int k = 0; k < Pieces.Count; k++)
                                    {
                                        Piece secondPieceToVerify = Pieces.ElementAt(k);
                                        if (secondPieceToVerify.Type == GameUtil.PieceType.EMPTY)
                                        {
                                            if (secondPieceToVerify.Row == currentPiece.Row + 2)
                                            {
                                                if (pieceToVerify.Column + sign == secondPieceToVerify.Column)
                                                {
                                                    if (!FoundPieceAtPosition(secondPieceToVerify.Row, secondPieceToVerify.Column))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool BlackKingShoodTakeInBack()
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Piece currentPiece = Pieces.ElementAt(i);

                if (currentPiece.Type == GameUtil.PieceType.BLACK_KING)
                {
                    for (int j = 0; j < Pieces.Count; j++)
                    {
                        Piece pieceToVerify = Pieces.ElementAt(j);
                        int sign = 0;

                        if (pieceToVerify.Type == GameUtil.PieceType.WHITE_QUEEN || pieceToVerify.Type == GameUtil.PieceType.WHITE_KING)
                        {
                            if (pieceToVerify.Row == currentPiece.Row - 1)
                            {
                                if (pieceToVerify.Column == currentPiece.Column + 1)
                                {
                                    sign = 1;
                                }
                                else if (pieceToVerify.Column == currentPiece.Column - 1)
                                {
                                    sign = -1;
                                }

                                if (sign != 0)
                                {
                                    for (int k = 0; k < Pieces.Count; k++)
                                    {
                                        Piece secondPieceToVerify = Pieces.ElementAt(k);
                                        if (secondPieceToVerify.Type == GameUtil.PieceType.EMPTY)
                                        {
                                            if (secondPieceToVerify.Row == currentPiece.Row - 2)
                                            {
                                                if (pieceToVerify.Column + sign == secondPieceToVerify.Column)
                                                {
                                                    if (!FoundPieceAtPosition(secondPieceToVerify.Row, secondPieceToVerify.Column))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region WhiteRegion
        private Piece WhiteKingCanTake(Piece futurePosition) 
        {
            if (futurePosition.Row == selectedPiece.Row + 2)
            {
                if ((futurePosition.Column == selectedPiece.Column - 2) || (futurePosition.Column == selectedPiece.Column + 2))
                {
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                        {
                            if (piece.Row == (selectedPiece.Row + 1))
                            {
                                if (piece.Type == GameUtil.PieceType.BLACK_QUEEN || piece.Type == GameUtil.PieceType.BLACK_KING)
                                {
                                    if (piece.Column + 1 == futurePosition.Column || piece.Column - 1 == futurePosition.Column)
                                    {
                                        return piece;
                                    }
                                }
                            }

                        }
                    }
                }

            }

            if (futurePosition.Row == selectedPiece.Row - 2)
            {
                if ((futurePosition.Column == selectedPiece.Column - 2) || (futurePosition.Column == selectedPiece.Column + 2))
                {
                    foreach (Piece piece in Pieces)
                    {
                        if (piece.Column == (selectedPiece.Column + 1) || piece.Column == (selectedPiece.Column - 1))
                        {
                            if (piece.Row == (selectedPiece.Row - 1))
                            {
                                if (piece.Type == GameUtil.PieceType.BLACK_QUEEN || piece.Type == GameUtil.PieceType.BLACK_KING)
                                {
                                    if (piece.Column + 1 == futurePosition.Column || piece.Column - 1 == futurePosition.Column)
                                    {
                                        return piece;
                                    }
                                }
                            }

                        }
                    }
                }

            }

            return null;
        }

        private bool WhiteKingShoodTake()
        {


            return WhiteKingShoodTakeInFront() || WhiteKingShoodTakeInBack();
        }

        private bool WhiteKingShoodTakeInFront() 
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Piece currentPiece = Pieces.ElementAt(i);

                if (currentPiece.Type == GameUtil.PieceType.WHITE_KING)
                {
                    for (int j = 0; j < Pieces.Count; j++)
                    {
                        Piece pieceToVerify = Pieces.ElementAt(j);
                        int sign = 0;

                        if (pieceToVerify.Type == GameUtil.PieceType.BLACK_QUEEN || pieceToVerify.Type == GameUtil.PieceType.BLACK_KING)
                        {
                            if (pieceToVerify.Row == currentPiece.Row + 1)
                            {
                                if (pieceToVerify.Column == currentPiece.Column + 1)
                                {
                                    sign = 1;
                                }
                                else if (pieceToVerify.Column == currentPiece.Column - 1)
                                {
                                    sign = -1;
                                }

                                if (sign != 0)
                                {
                                    for (int k = 0; k < Pieces.Count; k++)
                                    {
                                        Piece secondPieceToVerify = Pieces.ElementAt(k);
                                        if (secondPieceToVerify.Type == GameUtil.PieceType.EMPTY)
                                        {
                                            if (secondPieceToVerify.Row == currentPiece.Row + 2)
                                            {
                                                if (pieceToVerify.Column + sign == secondPieceToVerify.Column)
                                                {
                                                    if (!FoundPieceAtPosition(secondPieceToVerify.Row, secondPieceToVerify.Column))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool WhiteKingShoodTakeInBack()
        {
            for (int i = 0; i < Pieces.Count; i++)
            {
                Piece currentPiece = Pieces.ElementAt(i);

                if (currentPiece.Type == GameUtil.PieceType.WHITE_KING)
                {
                    for (int j = 0; j < Pieces.Count; j++)
                    {
                        Piece pieceToVerify = Pieces.ElementAt(j);
                        int sign = 0;

                        if (pieceToVerify.Type == GameUtil.PieceType.BLACK_QUEEN || pieceToVerify.Type == GameUtil.PieceType.BLACK_KING)
                        {
                            if (pieceToVerify.Row == currentPiece.Row - 1)
                            {
                                if (pieceToVerify.Column == currentPiece.Column + 1)
                                {
                                    sign = 1;
                                }
                                else if (pieceToVerify.Column == currentPiece.Column - 1)
                                {
                                    sign = -1;
                                }

                                if (sign != 0)
                                {
                                    for (int k = 0; k < Pieces.Count; k++)
                                    {
                                        Piece secondPieceToVerify = Pieces.ElementAt(k);
                                        if (secondPieceToVerify.Type == GameUtil.PieceType.EMPTY)
                                        {
                                            if (secondPieceToVerify.Row == currentPiece.Row - 2)
                                            {
                                                if (pieceToVerify.Column + sign == secondPieceToVerify.Column)
                                                {
                                                    if (!FoundPieceAtPosition(secondPieceToVerify.Row, secondPieceToVerify.Column))
                                                    {
                                                        return true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        private bool FoundPieceAtPosition(int row, int column)
        {
            foreach (Piece piece in Pieces)
            {
                if (piece.Column == column && piece.Row == row && piece.Type != GameUtil.PieceType.EMPTY)
                {
                    return true;
                }
            }

            return false;
        }

        private void TransformKings()
        {
            foreach(Piece piece in Pieces)
            {
                int rowForBlack = 7;
                int rowForWhite = 0;
                if(piece.Type == GameUtil.PieceType.BLACK_QUEEN)
                {
                    if(piece.Row == rowForBlack)
                    {
                        piece.ImageSource = ConvertImage(@"\Images\black_queen.png");
                        piece.Type = GameUtil.PieceType.BLACK_KING;
                    }
                }
                else if (piece.Type == GameUtil.PieceType.WHITE_QUEEN)
                {
                    if (piece.Row == rowForWhite)
                    {
                        piece.ImageSource = ConvertImage(@"\Images\white_queen.png");
                        piece.Type = GameUtil.PieceType.WHITE_KING;
                    }
                }
            }
        }

        private BitmapImage ConvertImage(string source)
        {
            BitmapImage ImageSource = new BitmapImage();

            ImageSource.BeginInit();
            ImageSource.UriSource = new Uri(source, UriKind.RelativeOrAbsolute);
            ImageSource.EndInit();

            return ImageSource;
        }
    }
}
