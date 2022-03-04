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
    public TMP_InputField nameTMP;
    public TextMeshProUGUI hiscoreTMP;

    public ConnectionManager connectionManager;

    private void Start()
    {
        UserManager.onUserUpdated += SetCredentials;
        SetCredentials();
    }

    private void SetCredentials()
    {
        usernameTMP.text = UserManager.user.username;
        nameTMP.text = UserManager.user.username;
        SetScore(UserManager.high_score);
    }

    public void SetUsername()
    {
        UserManager.UpdateUsername(nameTMP.text);
    }

    public void SetScore(int amount)
    {
        scoreTMP.text = scorePrefix + amount.ToString();
        hiscoreTMP.text = amount.ToString();
    }
}
