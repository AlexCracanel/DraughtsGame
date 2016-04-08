using DraughtsGame.Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.BinarySerializer
{
    [Serializable]
    class ObjectToSerialize : ISerializable
    {
        public ObservableCollection<Piece> Pieces;
        public int BlackScore;
        public int WhiteScore;
        public GameUtil.CurrentPlayer CurrentPlayer;
        public string InfoMenuText;

        public ObjectToSerialize() { }

        public ObjectToSerialize(SerializationInfo info, StreamingContext ctxt)
        {

            Pieces = (ObservableCollection<Piece>)info.GetValue("pices", typeof(ObservableCollection<Piece>));
            BlackScore = (int)info.GetValue("blackScore", typeof(int));
            WhiteScore = (int)info.GetValue("whiteScore", typeof(int));
            CurrentPlayer = (GameUtil.CurrentPlayer)info.GetValue("currentPlayer", typeof(GameUtil.CurrentPlayer));
            InfoMenuText = (string)info.GetValue("infoMenuText", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("pices", Pieces);
            info.AddValue("blackScore", BlackScore);
            info.AddValue("whiteScore", WhiteScore);
            info.AddValue("currentPlayer", CurrentPlayer);
            info.AddValue("infoMenuText", InfoMenuText);
        }
    }
}
