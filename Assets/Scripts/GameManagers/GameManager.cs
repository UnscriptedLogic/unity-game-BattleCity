using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    public static GameManager instance;
    public event Action onGameStarted;
    public event Action onGameOver;

    public GameObject[] enableAfterInitialize;

    [Header("Effect Options")]
    public bool playEffects;
    public bool playSounds;
    public bool playParticles;

    [Header("Home Base")]
    public List<Transform> baseWalls = new List<Transform>();
    [HideInInspector] public List<Vector3> baseWallPositions = new List<Vector3>();

    private void Awake()
    {
        instance = this;
        InitializeSephamores();
    }

    public void LoadSaves()
    {
        SaveManager.Load();
    }

    private void OnApplicationQuit()
    {
        SaveManager.Save();
    }

    #region Standalone Functions
    public void EnableObjects()
    {
        for (int i = 0; i < enableAfterInitialize.Length; i++)
        {
            enableAfterInitialize[i].SetActive(true);   
        }

        for (int i = 0; i < baseWalls.Count; i++)
        {
            baseWallPositions.Add(baseWalls[i].position);
            baseWalls[i].GetChild(0).GetChild(0).GetComponent<EntityHealth>().onDeathDisables = true;
            baseWalls[i].GetChild(0).GetChild(0).GetComponent<EntityHealth>().onDeathDestroys = false;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        onGameOver?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    } 
    #endregion
}
