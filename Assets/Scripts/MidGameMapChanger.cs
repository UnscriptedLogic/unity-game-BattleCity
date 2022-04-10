using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MidGameMapChanger : Semaphore
{
    [SerializeField] private PointShopWaveSpawner spawner;
    [SerializeField] private int wavesToChange;
    [SerializeField] private Vector2Int mapBuildIndexRange;
    private int waves;
    private int mapIndex;

    protected override void SephamoreStart(Manager manager)
    {
        mapIndex = SceneManager.GetSceneAt(1).buildIndex;
        spawner.onWaveStarting += OnWaveCompleted;
    }

    public void OnWaveCompleted()
    {
        waves++;
        if (waves >= wavesToChange)
        {
            mapIndex++;
            if (mapIndex > mapBuildIndexRange.y)
            {
                mapIndex = mapBuildIndexRange.x;
            }

            SceneManager.LoadSceneAsync(mapIndex, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            waves = 0;
        }
    }
}
