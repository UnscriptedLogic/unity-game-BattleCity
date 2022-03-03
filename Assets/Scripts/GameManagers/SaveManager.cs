using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    public static string saveFilename = "save.unscripted";
    public static string savePath = Application.persistentDataPath + "/" + saveFilename;

    public static SaveData savedData;
    public static string jsonString;

    public static void GetData()
    {
        //Get data file
        if (File.Exists(savePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonString);
        }

        //if no data file present, create file
        //Initialize new saves
        //else if data file present
        //load data into Global vars
    }

    public static void Save()
    {
        SaveData saveData = new SaveData
        {
            username = UserManager.user.username
        };

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(savePath, FileMode.Create);

        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    public static void Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(savePath, FileMode.Open);

            SaveData loadedData = binaryFormatter.Deserialize(fileStream) as SaveData;
            fileStream.Close();

            savedData = loadedData;
        } else
        {
            Debug.Log("No save file detected, creating one now");
            SaveData newData = new SaveData
            {
                username = UserManager.user.username
            };

            savedData = newData;
        }
    }

    public class SaveData
    {
        public string username;
    }

}
