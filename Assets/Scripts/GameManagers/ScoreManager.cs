using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : Semaphore
{
    public static ScoreManager instance;
    public Vector2 scoreClamp = new Vector2(0, 99999);
    public int scorePerHit = 25;

    public Dictionary<int, int> entityScores = new Dictionary<int, int>();
    public event Action<int, int> onTankScoreUpdated;


    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        instance = this;
        DamageManager.onTankHitEvent += DamageManager_onTankHitEvent;
    }

    private void DamageManager_onTankHitEvent(int amount, TankManager culprit, TankManager victim)
    {
        if (culprit)
        {
            TankScore tankScore = culprit.GetComponent<TankScore>();
            if (tankScore)
            {
                AddScore(culprit.tankIndex, scorePerHit * amount);
            }
        }
    }

    public void UpdateScore(int entityIndex, int score)
    {
        if (entityScores.ContainsKey(entityIndex))
        {
            entityScores[entityIndex] = score;
        } else
        {
            entityScores.Add(entityIndex, score);
        }

        onTankScoreUpdated?.Invoke(entityIndex, score);
    }

    public void AddScore(int entityIndex, int score)
    {
        if (entityScores.ContainsKey(entityIndex))
        {
            entityScores[entityIndex] += score;
        }
        else
        {
            entityScores.Add(entityIndex, score);
        }

        onTankScoreUpdated?.Invoke(entityIndex, entityScores[entityIndex]);
    }
}
