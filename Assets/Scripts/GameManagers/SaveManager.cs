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
        Debug.Log(Application.persistentDataPath   );

    }

    public static void Save()
    {
        SaveData saveData = new SaveData
        {
            username = UserManager.GetUser().username,
            highest_wave = UserManager.GetUser().highest_wave,
            experience = UserManager.GetUser().experience,
            level = UserManager.GetUser().level
        };

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(savePath, FileMode.Create);

        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
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
                highest_wave = UserManager.GetUser().highest_wave,
                experience = UserManager.GetUser().experience,
                level = UserManager.GetUser().level
            };

            savedData = newData;
        }
        Load();
    }

    public static void Load()
    {
        UserManager.CreateUser(savedData.username);

        UserManager.user.experience = savedData.experience;
        UserManager.user.level = savedData.level;
        UserManager.user.highest_wave = savedData.highest_wave;

        ExperienceManager.SetExpLevel(savedData.experience, savedData.level);

        onDataLoaded?.Invoke();
    }

    [Serializable]
    public class SaveData
    {
        public string username;
        public int highest_wave;
        public int experience;
        public int level;
    }
}
