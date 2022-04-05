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

    public void UpdateScores(int tankIndex)
    {
        //Updates only when there is a score card reference in the dictionary
        if (entityUIScores.TryGetValue(tankIndex, out ScoreCard scoreCard))
        {
            scoreCard.SetScore(scoreManager.entityScores[tankIndex]);
            return;
        }

        CreateScore(tankIndex);
    }

    public void CreateScore(int entityIndex)
    {
        //This creates the card that appears at the top right of the screen
        GameObject scoreCardGO = Instantiate(scoreCardPrefab, scoreParent);
        ScoreCard scoreCardscript = scoreCardGO.GetComponent<ScoreCard>();
        scoreCardscript.entityIndex = entityIndex;

        entityUIScores.Add(entityIndex, scoreCardscript);
        scoreCardscript.SetScore(scoreManager.entityScores[entityIndex]);
        if (UserManager.user != null)
        {
            scoreCardscript.SetName(UserManager.user.username);
        }
    }
}