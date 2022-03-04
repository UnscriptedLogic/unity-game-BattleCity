using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("All game maps")]
    public List<Scene> maps;

    public string gameMode;
    public string gameMap;

    public void SetGameMode(string modeName)
    {
        gameMode = modeName;
    }

    public void SetGameMap(string mapName)
    {
        gameMap = mapName;
    }

    public void LoadGame()
    {
        //Loads the mode first because that is where all the game managers are
        SceneManager.LoadSceneAsync(gameMode, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(gameMap, LoadSceneMode.Additive);
    }

    public void ChangeGameMap(string currentMap, string newMap)
    {
        SceneManager.UnloadSceneAsync(currentMap);
        SceneManager.LoadSceneAsync(newMap, LoadSceneMode.Additive);
        gameMap = newMap;
    }

    public void NextMap()
    {
        //Check where we are in the list of maps
        for (int i = 0; i < maps.Count; i++)
        {
            //Find our location in that list of maps
            if (maps[i].name == gameMap)
            {
                //Check if we are the last in line
                int nextMapIndex = i + 1;
                if (nextMapIndex > maps.Count)
                {
                    //Reset to first map if we are
                    nextMapIndex = 0;
                }

                ChangeGameMap(gameMap, maps[nextMapIndex].name);
                return;
            }
        }
    }
}
