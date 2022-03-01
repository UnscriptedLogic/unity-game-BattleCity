﻿using System;
using System.Collections;
using UnityEngine;

public enum ModifyScoreType
{
    Kill,
    Killed,
    StayedAlive,
    None
}

[Serializable]
public class ModifyScore
{
    public ModifyScoreType modifyType;
    public int amount;
}

public class TankScore : EntitySemaphore
{
    public int score;
    public ModifyScore[] scoreModifiers;
    public TankManager tankManager;
    private TankHealth healthScript;
    private ScoreManager scoreManager;

    public override void Initialize(EntityManager manager)
    {
        scoreManager = ScoreManager.instance;

        healthScript = tankManager.healthScript;
        healthScript.onKill += OnKill;
        healthScript.onKilled += OnKilled;
        ModifyScore(ModifyScoreType.None);

        base.Initialize(manager);
    }

    private void OnKilled(EntityManager source)
    {
        ModifyScore(ModifyScoreType.Killed);
    }

    private void OnKill(EntityManager source)
    {
        ModifyScore(ModifyScoreType.Kill);
    }

    private void ModifyScore(ModifyScoreType modifyScoreType)
    {
        for (int i = 0; i < scoreModifiers.Length; i++)
        {
            if (scoreModifiers[i].modifyType == modifyScoreType)
            {
                score += scoreModifiers[i].amount;
                score = (int)Mathf.Clamp(score, scoreManager.scoreClamp.x, scoreManager.scoreClamp.y);

                scoreManager.UpdateScore(tankManager.tankIndex, score);
            }
        }
    }
}