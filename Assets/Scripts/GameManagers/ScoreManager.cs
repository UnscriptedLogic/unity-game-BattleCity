using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreManager : Manager
{
    public static ScoreManager instance;
    public Dictionary<int, int> entityScores = new Dictionary<int, int>();

    public event Action<int> onEntityScoreUpdated;

    public Vector2 scoreClamp = new Vector2(0, 99999);

    //public override void Initialize()
    //{
    //    instance = this;

    //    base.Initialize();
    //}

    public void UpdateScore(int entityIndex, int score)
    {
        if (entityScores.ContainsKey(entityIndex))
        {
            entityScores[entityIndex] = score;
        } else
        {
            entityScores.Add(entityIndex, score);
        }

        onEntityScoreUpdated?.Invoke(entityIndex);
    }
}
