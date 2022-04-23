using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStoreScore : MonoBehaviour
{
    public ScoreManager scoreManager;
    public PointShopWaveSpawner waveSpawner;

    private void Start()
    {
        GameManager.instance.onGameOver += delegate ()
        {
            Debug.Log("Game Over");


            if (waveSpawner.TotalWaves > UserManager.GetUser().highest_wave)
            {
                UserManager.UpdateWaveScore(waveSpawner.TotalWaves);
            }

            ExperienceManager.AddExperience(scoreManager.entityScores.ElementAt(0).Value);
            Debug.Log($"Added Experience: {scoreManager.entityScores.ElementAt(0).Value}");
            Debug.Log($"Current Experience: {ExperienceManager.Experience}");
        };
    }
}