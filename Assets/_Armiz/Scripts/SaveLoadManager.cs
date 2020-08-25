using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Armiz
{
    public class SaveLoadManager
    {
        private string defaultSavePath = Application.persistentDataPath + "/Data.dat";
        public void SaveThisData(FightersSaveData data)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                FileStream fileStream = File.Create(defaultSavePath);
                bf.Serialize(fileStream, data);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception in SaveLoadManager-SaveThisData: " + ex.Message);
            }
        }
        public FightersSaveData LoadSavedData()
        {
            try
            {
                FileStream fileStream = File.Open(defaultSavePath, FileMode.Open);
                if (File.Exists(defaultSavePath) && fileStream.Length > 0)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    //GameSave data = (GameSave)bf.Deserialize(fileStream);
                    FightersSaveData data = bf.Deserialize(fileStream) as FightersSaveData;
                    fileStream.Close();

                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception in SaveLoadManager-LoadSavedData: " + ex.Message);
                return null;
            }
        }
    }
}
