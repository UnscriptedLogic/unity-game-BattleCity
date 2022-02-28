using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Score")]
    public Transform content;
    public GameObject scoreCardPrefab;
    public Button reloadScoreButton;

    public Button leaderboardButton;
    public Color myColor;

    public ConnectionManager connManager;

    public Dictionary<string, GameObject> scoreCards;

    private void Start()
    {
        if (connManager.initialized)
        {
            StartCoroutine(connManager.CheckConnection((value) =>
            {
                leaderboardButton.interactable = value;
            }));

            reloadScoreButton.onClick.AddListener(() =>
            {
                PerformDisplayScore();
            });

            PerformDisplayScore();

            connManager.onPlayerInitialized += delegate (Player player)
            {
                ColorMyName(player);
            };
        }
    }

    private void ColorMyName(Player player)
    {
        if (player.initialized)
        {
            if (scoreCards.ContainsKey(player.username))
            {
                scoreCards[player.username].transform.GetChild(0).GetComponent<Image>().color = myColor;
            }
        }
    }

    public void PerformDisplayScore()
    {
        StartCoroutine(connManager.PerformRequest(connManager.scoresRoute, (res) =>
        {
            DisplayScores(res);

            ColorMyName(connManager.player);
        }));
    }

    private void DisplayScores(string res)
    {
        scoreCards = new Dictionary<string, GameObject>();

        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        string[] scores = res.Split("|");
        for (int i = 0; i < scores.Length - 1; i++)
        {
            string username = scores[i].Split(":")[0];
            string score = scores[i].Split(":")[1];

            GameObject scoreCard = Instantiate(scoreCardPrefab, content);
            scoreCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = username;
            scoreCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = score;

            scoreCards.Add(username, scoreCard);
        }
    }
}