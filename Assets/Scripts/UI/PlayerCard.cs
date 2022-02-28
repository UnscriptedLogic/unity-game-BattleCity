using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    [Header("Mini Card")]
    public TextMeshProUGUI usernameTMP;
    public TextMeshProUGUI scoreTMP;
    public string scorePrefix = "Score: ";

    [Header("Detailed Card")]
    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI hiscoreTMP;

    public ConnectionManager connectionManager;

    private void Awake()
    {
        connectionManager.onInitialized += delegate (Manager manager)
        {
            connectionManager.onPlayerInitialized += delegate (Player player)
            {
                usernameTMP.text = player.username;
                nameTMP.text = usernameTMP.text;

                SetScore(player.hiscore);
            };
        };
    }

    public void SetScore(int amount)
    {
        scoreTMP.text = scorePrefix + amount.ToString();
        hiscoreTMP.text = amount.ToString();
    }
}
