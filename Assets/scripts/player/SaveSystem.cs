using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem 
{
    private static SaveData saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        public PlayerData playerData;
        //public PlayerInvetory ItensData;
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public static void Save()
    {
       HandleSaveData(); 
       File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData, true));
    }

    private static void HandleSaveData()
    {
        FirstPersonController.Instance.Save(ref saveData.playerData);
        //saveData.ItensData = InventoryManager.Instance.GetPlayerInventoryData();
    }

    public static void Load()
    {
        if (File.Exists(SaveFileName()))
        {
            string saveFile = File.ReadAllText(SaveFileName());
            saveData = JsonUtility.FromJson<SaveData>(saveFile);
            HandleLoadData();
        }
        else
        {
            Debug.LogWarning("Arquivo de save não encontrado.");
        }
    }

    private static void HandleLoadData()
    {
        FirstPersonController.Instance.Load(saveData.playerData);
        //InventoryManager.Instance.SetPlayerInventoryData(saveData.ItensData);
    }
}
