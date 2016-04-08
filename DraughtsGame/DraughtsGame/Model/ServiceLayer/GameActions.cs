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
using System.Windows.Documents;
using DraughtsGame.View.GameTableUserControl.ViewModel;
using DraughtsGame.BinarySerializer;
using Microsoft.Win32;
using System.Windows.Data;

namespace DraughtsGame.Model.ServiceLayer
{
    class GameActions : DependencyObject
    {

        private bool isGameOver;

        public int BlackScore
        {
            get { return (int)GetValue(BlackScoreProperty); }
            set { SetValue(BlackScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BlackScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlackScoreProperty =
            DependencyProperty.Register("BlackScore", typeof(int), typeof(GameActions), new PropertyMetadata(0,new PropertyChangedCallback(BlackScore_PropertyChangedCallback)));

        static void BlackScore_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            GameActions myObject = d as GameActions;
            Console.WriteLine("InChange");

            if (myObject != null)
            {
                if(myObject.BlackScore == 12)
                {
                    myObject.InfoMenuText = "Black player win !";
                    myObject.isGameOver = true;
                }
            }
        }

        public int WhiteScore
        {
            get { return (int)GetValue(WhiteScoreProperty); }
            set { SetValue(WhiteScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WhiteScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WhiteScoreProperty =
            DependencyProperty.Register("WhiteScore", typeof(int), typeof(GameActions), new PropertyMetadata(0,new PropertyChangedCallback(WhiteScore_PropertyChangedCallback)));

        static void WhiteScore_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GameActions myObject = d as GameActions;
            Console.WriteLine("InChange");

            if (myObject != null)
            {
                if (myObject.WhiteScore == 12)
                {
                    myObject.InfoMenuText = "White player win !";
                    myObject.isGameOver = true;
                }
            }
        }

        public string InfoMenuText
        {
            get { return (string)GetValue(InfoTextProperty); }
            set { SetValue(InfoTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InfoText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register("InfoMenuText", typeof(string), typeof(GameActions), new PropertyMetadata(String.Empty));



        public ObservableCollection<Piece> Pieces
        {
            get { return (ObservableCollection<Piece>)GetValue(PiecesProperty); }
            set { SetValue(PiecesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pieces.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PiecesProperty =
            DependencyProperty.Register("Pieces", typeof(ObservableCollection<Piece>), typeof(GameActions), new PropertyMetadata(new ObservableCollection<Piece>()));


        private Piece selectedPiece;

        public GameUtil.CurrentPlayer currentPlayer;

        public GameActions()
        {
            Pieces = new ObservableCollection<Piece>();

            AddEmptyQueen();
            AddBlackQueen();
            AddWhiteQueen();

            isGameOver = false;

            currentPlayer = GameUtil.CurrentPlayer.BLACK_PLAYER;
            InfoMenuText = "Black player turn !";
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
            if(isGameOver)
            {
                return;
            }

            var piece = parameter as Piece;

            if(piece == null)
            {
                return;
            }

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
                    InfoMenuText = "White player turn !";

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

                if (!BlackShoodTake() && !BlackKingShoodTake() && !isGameOver)
                {
                    currentPlayer = GameUtil.CurrentPlayer.WHITE_PLAYER;
                    InfoMenuText = "White player turn !";
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
                    InfoMenuText = "Black player turn !";

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

                if (!WhiteShoodTake() && !WhiteKingShoodTake() && !isGameOver)
                {
                    currentPlayer = GameUtil.CurrentPlayer.BLACK_PLAYER;
                    InfoMenuText = "Black player turn !";
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

        public void SaveGame(object parameter)
        {
            GameTableViewModel gameTableViewModel  = parameter as GameTableViewModel;

            ObjectToSerialize obj = new ObjectToSerialize();

            obj.BlackScore = gameTableViewModel.BlackScore;
            obj.Pieces = gameTableViewModel.Pieces;
            obj.WhiteScore = gameTableViewModel.WhiteScore;
            obj.CurrentPlayer = gameTableViewModel.currentPlayer;
            obj.InfoMenuText = gameTableViewModel.InfoMenuText;

            Serializer serializer = new Serializer();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.Title = "Save game";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                serializer.SerializeObject(@saveFileDialog.FileName, obj);
            }
        }

        public void LoadGame(object parameter)
        {
            GameTableViewModel gameTableViewModel = parameter as GameTableViewModel;

            ObjectToSerialize obj = new ObjectToSerialize();
            Serializer serializer = new Serializer();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = true;

            bool? userClickedOK = openFileDialog.ShowDialog();

            if (userClickedOK == true)
            {
                obj = serializer.DeserializeObject(@openFileDialog.FileName);

                gameTableViewModel.Pieces = obj.Pieces;
                gameTableViewModel.BlackScore = obj.BlackScore;
                gameTableViewModel.WhiteScore = obj.WhiteScore;
                gameTableViewModel.InfoMenuText = obj.InfoMenuText;
                gameTableViewModel.currentPlayer = obj.CurrentPlayer;

                RedrawPieceImages();
            }
        }

        private void RedrawPieceImages()
        {
            foreach(Piece piece in Pieces)
            {
                if(piece.Type == GameUtil.PieceType.WHITE_QUEEN)
                {
                    piece.ImageSource = ConvertImage(@"\Images\whiteChecker.png");
                }
                else if (piece.Type == GameUtil.PieceType.BLACK_QUEEN)
                {
                    piece.ImageSource = ConvertImage(@"\Images\blackCheker.png");
                }
                else if (piece.Type == GameUtil.PieceType.BLACK_KING)
                {
                    piece.ImageSource = ConvertImage(@"\Images\black_queen.png");
                }
                else if (piece.Type == GameUtil.PieceType.WHITE_KING)
                {
                    piece.ImageSource = ConvertImage(@"\Images\white_queen.png");
                }

            }
        }
    }
}
