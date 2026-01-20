using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveLoadManager : Singleton<SaveLoadManager>
{
    SerializedData data = new SerializedData();

    public void SaveData()
    {
        data.ser_songTempo = GameData.songTempo;
        data.ser_songsCompleted = GameData.songsCompleted;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameData.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            data = (SerializedData)bf.Deserialize(file);
            file.Close();

            GameData.songTempo = data.ser_songTempo;
            GameData.songsCompleted = data.ser_songsCompleted;
        }
    }
}