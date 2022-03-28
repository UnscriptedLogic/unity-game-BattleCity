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
    public event Action onGameWon;

    public GameObject[] enableAfterInitialize;

    [Header("Effect Options")]
    public bool playEffects;
    public bool playSounds;
    public bool playParticles;

    private void Awake()
    {
        instance = this;
        InitializeSephamores();
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
    }

    public void GameWon()
    {
        Debug.Log("Game Won");
        onGameWon?.Invoke();
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
