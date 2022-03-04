using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Semaphore
{
    public static string saveFilename = "save.unscripted";
    public static string savePath;

    public static SaveData savedData;
    public static string jsonString;

    public static event Action onDataLoaded;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        savePath = Application.persistentDataPath + "/" + saveFilename;
        GetSavedData();
    }

    public static void Save()
    {
        SaveData saveData = new SaveData
        {
            username = UserManager.GetUser().username,
            high_score = UserManager.high_score
        };

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(savePath, FileMode.Create);

        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();

        Debug.Log(savedData.username);
        Debug.Log("Saved");
    }

    public static void GetSavedData()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(savePath, FileMode.Open);

            savedData = binaryFormatter.Deserialize(fileStream) as SaveData;
            fileStream.Close();
        } else
        {
            Debug.Log("No save file detected, creating one now");
            SaveData newData = new SaveData
            {
                username = UserManager.GetUser().username,
                high_score = UserManager.high_score
            };

            savedData = newData;
        }

        Debug.Log("Data Loaded!");
        Load();
    }

    public static void Load()
    {
        UserManager.CreateUser(new UserManager.User 
        { 
            username = savedData.username,
        });
        
        UserManager.high_score = savedData.high_score;
        onDataLoaded?.Invoke();

        Debug.Log(savedData.username);
        Debug.Log("Loaded");
    }

    [Serializable]
    public class SaveData
    {
        public string username;
        public int high_score;
    }
}
