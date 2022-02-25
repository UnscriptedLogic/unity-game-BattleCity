using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    public static GameManager instance;
    SemaphoreSlim gate = new SemaphoreSlim(1);

    public event Action onGameStarted;
    public event Action onGameOver;

    [Header("Initialization")]
    public Manager[] initalizeOrder;
    public GameObject[] enableAfterInitialize;

    [Header("Effect Options")]
    public bool playEffects;
    public bool playSounds;
    public bool playParticles;

    [Header("Home Base")]
    public List<Transform> baseWalls = new List<Transform>();
    [HideInInspector] public List<Vector3> baseWallPositions = new List<Vector3>();

    private async void InitializeScripts()
    {
        instance = this;
        for (int i = 0; i < initalizeOrder.Length; i++)
        {
            await gate.WaitAsync();
            initalizeOrder[i].Initialize();
        }

        onGameStarted?.Invoke();
        Debug.Log("All managers loaded!");


        for (int i = 0; i < enableAfterInitialize.Length; i++)
        {
            enableAfterInitialize[i].SetActive(true);   
        }

        for (int i = 0; i < baseWalls.Count; i++)
        {
            baseWallPositions.Add(baseWalls[i].position);
            baseWalls[i].GetComponent<BlockManager>().blockHealth.onDeathDisables = true;
            baseWalls[i].GetComponent<BlockManager>().blockHealth.onDeathDestroys = false;
        }
    }

    public void ReleaseGate(Manager manager)
    {
        gate.Release();
    }

    private void Awake()
    {
        InitializeScripts();
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
}
