using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.BinarySerializer
{
    class Serializer
    {
        public void SerializeObject(string filename, ObjectToSerialize objectToSerialize)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
            }
        }

        public ObjectToSerialize DeserializeObject(string filename)
        {
            ObjectToSerialize objectToSerialize;
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (ObjectToSerialize)bFormatter.Deserialize(stream);
            }
            return objectToSerialize;
        }
    }
}
