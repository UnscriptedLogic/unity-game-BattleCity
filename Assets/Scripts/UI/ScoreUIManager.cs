using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ScoreUIManager : Semaphore
{
    public ScoreManager scoreManager;
    private TankIndexManager indexManager;

    public Dictionary<int, ScoreCard> entityUIScores = new Dictionary<int, ScoreCard>();
    public GameObject scoreCardPrefab;
    public Transform scoreParent;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        scoreManager.onTankScoreUpdated += ScoreManager_onTankScoreUpdated;
    }

    private void ScoreManager_onTankScoreUpdated(int tankIndex, int score)
    {
        UpdateScores(tankIndex);
    }

    public void CheckScores()
    {
        for (int i = 0; i < scoreManager.entityScores.Count; i++)
        {
            int entityIndex = scoreManager.entityScores.ElementAt(i).Key;

            if (entityUIScores.ContainsKey(entityIndex) == false)
            {
                CreateScore(entityIndex);
                return;
            }

            UpdateScores(entityIndex);
        }
    }

    public void UpdateScores(int tankIndex)
    {
        if (entityUIScores.TryGetValue(tankIndex, out ScoreCard scoreCard))
        {
            scoreCard.SetScore(scoreManager.entityScores[tankIndex]);
            //scoreCard.SetName(indexManager.tankIndexes[tankIndex].name);
            return;
        }

        CreateScore(tankIndex);
    }

    public void CreateScore(int entityIndex)
    {
        GameObject scoreCardGO = Instantiate(scoreCardPrefab, scoreParent);
        ScoreCard scoreCardscript = scoreCardGO.GetComponent<ScoreCard>();
        scoreCardscript.entityIndex = entityIndex;

        entityUIScores.Add(entityIndex, scoreCardscript);
        scoreCardscript.SetScore(scoreManager.entityScores[entityIndex]);
        scoreCardscript.SetName(GlobalVars.player != null ? GlobalVars.player.username : "Player");
    }
}