using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PianoTiles.Local
{
    public static class SaveSystem
    {
        public static void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            string path = Application.persistentDataPath + "/data.bin";
            FileStream stream = new FileStream(path, FileMode.Create);

            SavedData data = new SavedData();

            bf.Serialize(stream, data);
            stream.Close();
        }

        public static SavedData Load()
        {
            string path = Application.persistentDataPath + "/data.bin";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = null;
            SavedData data = null;

            if (File.Exists(path))
            {
                stream = new FileStream(path, FileMode.Open);

                data = bf.Deserialize(stream) as SavedData;
                stream.Close();

                return data;
            }
            stream = new FileStream(path, FileMode.Create);

            data = new SavedData(true);
            bf.Serialize(stream, data);
            stream.Close();
            return data;
        }
    }
}
