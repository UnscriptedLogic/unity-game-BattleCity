using System;
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

public class TankScore : Semaphore
{
    public int score;
    private TankManager tankManager;
    private ScoreManager scoreManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        tankManager = manager as TankManager;
        scoreManager = ScoreManager.instance;
        scoreManager.onTankScoreUpdated += ScoreManager_onTankScoreUpdated;
    }

    public void InitializeScore()
    {
        scoreManager.UpdateScore(tankManager.tankIndex, score);
    }

    private void ScoreManager_onTankScoreUpdated(int entityIndex, int score)
    {
        if (entityIndex == tankManager.tankIndex)
        {
            this.score = score;
        }
    }

    //public override void Initialize(EntityManager manager)
    //{
    //    scoreManager = ScoreManager.instance;

    //    healthScript = tankManager.healthScript;
    //    healthScript.onKill += OnKill;
    //    healthScript.onKilled += OnKilled;
    //    ModifyScore(ModifyScoreType.None);

    //    base.Initialize(manager);
    //}

    //private void OnKilled(EntityManager source)
    //{
    //    ModifyScore(ModifyScoreType.Killed);
    //}

    //private void OnKill(EntityManager source)
    //{
    //    ModifyScore(ModifyScoreType.Kill);
    //}

    //private void ModifyScore(ModifyScoreType modifyScoreType)
    //{
    //    for (int i = 0; i < scoreModifiers.Length; i++)
    //    {
    //        if (scoreModifiers[i].modifyType == modifyScoreType)
    //        {
    //            score += scoreModifiers[i].amount;
    //            score = (int)Mathf.Clamp(score, scoreManager.scoreClamp.x, scoreManager.scoreClamp.y);

    //            scoreManager.UpdateScore(tankManager.tankIndex, score);
    //        }
    //    }
    //}
}