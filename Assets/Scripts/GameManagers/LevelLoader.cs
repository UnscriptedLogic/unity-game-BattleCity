using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Semaphore
{
    [Header("All game maps")]
    public List<string> mapNames;
    public int startingIndex = 2;
    public string gameMode;
    public string gameMap;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);

        for (int i = startingIndex; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
            mapNames.Add(sceneName);

        }
    }

    #region Button Selecting
    public void SetGameMode(string modeName)
    {
        gameMode = modeName;
    }

    public void SetGameMap(string mapName)
    {
        gameMap = mapName;
    } 
    #endregion

    public void LoadGame()
    {
        //Loads the mode first because that is where all the game managers are
        SceneManager.LoadSceneAsync(gameMode, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(gameMap, LoadSceneMode.Additive);
    }

    //Changing the current scene map
    public void ChangeGameMap(string newMap)
    {
        if (gameMap != "")
        {
            SceneManager.UnloadSceneAsync(gameMap);
        }

        SceneManager.LoadSceneAsync(newMap, LoadSceneMode.Additive);
        gameMap = newMap;
    }

    public void LoadRandomMap()
    {
        ChangeGameMap(RandomValue.FromList(mapNames.ToArray()));
    }

    //For going through all possible maps
    public void NextMap()
    {
        for (int i = 0; i < mapNames.Count; i++)
        {
            if (mapNames[i] == gameMap)
            {
                int nextMapIndex = i + 1;
                if (nextMapIndex > mapNames.Count)
                {
                    nextMapIndex = 0;
                }

                ChangeGameMap(mapNames[nextMapIndex]);
                return;
            }
        }
    }
}
