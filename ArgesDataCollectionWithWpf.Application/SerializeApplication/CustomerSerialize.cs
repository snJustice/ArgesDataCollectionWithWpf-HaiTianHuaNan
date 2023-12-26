//zy


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ArgesDataCollectionWithWpf.Application.SerializeApplication
{
    public static  class CustomerSerialize
    {
        

        static MemoryStream memory = new MemoryStream(); 
        public static byte[] SerializeToBinary(object serializeModel,Type type)
        {
            memory = new MemoryStream();
            //memory.Flush();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memory, serializeModel);
            return memory.GetBuffer();
        }


        public static object DeSerializeBinaryToObject(byte[] serializeData )
        {
            memory = new MemoryStream();
            memory.Write(serializeData,0, serializeData.Count());

            memory.Position = 0;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return binaryFormatter.Deserialize(memory);
        }

    }
}
