using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ScoreUIManager : Manager
{
    private ScoreManager scoreManager;
    private IndexManager indexManager;

    public Dictionary<int, ScoreCard> entityUIScores = new Dictionary<int, ScoreCard>();
    public GameObject scoreCardPrefab;
    public Transform scoreParent;

    public override void Initialize()
    {
        indexManager = IndexManager.instance;
        scoreManager = ScoreManager.instance;
        scoreManager.onEntityScoreUpdated += UpdateScores;

        base.Initialize();
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

    public void UpdateScores(int entityIndex)
    {
        if (entityUIScores.TryGetValue(entityIndex, out ScoreCard scoreCard))
        {
            scoreCard.SetScore(scoreManager.entityScores[entityIndex]);
            scoreCard.SetName(indexManager.entityIndexes[entityIndex].name);
            return;
        }

        CreateScore(entityIndex);
    }

    public void CreateScore(int entityIndex)
    {
        GameObject scoreCardGO = Instantiate(scoreCardPrefab, scoreParent);
        ScoreCard scoreCardscript = scoreCardGO.GetComponent<ScoreCard>();
        scoreCardscript.entityIndex = entityIndex;

        entityUIScores.Add(entityIndex, scoreCardscript);
        scoreCardscript.SetScore(scoreManager.entityScores[entityIndex]);
        scoreCardscript.SetName(GlobalVars.player.username);
    }
}