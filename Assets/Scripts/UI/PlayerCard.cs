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

    public Slider expSlider;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI leveltext;
    public float expLerpSpeed = 2f;

    private void Start()
    {
        UserManager.onUserUpdated += SetCredentials;
        SetCredentials();
    }

    private void Update()
    {
        expSlider.value = Mathf.Lerp(expSlider.value, ExperienceManager.Experience, expLerpSpeed);
    }

    private void SetCredentials()
    {
        usernameTMP.text = UserManager.user.username;
        nameTMP.text = UserManager.user.username;
        SetScore(UserManager.user.highest_wave);
    }

    public void SetUsername()
    {
        UserManager.UpdateUsername(nameTMP.text);
    }

    public void SetScore(int amount)
    {
        scoreTMP.text = scorePrefix + amount.ToString();
        hiscoreTMP.text = amount.ToString();

        leveltext.text = ExperienceManager.Level.ToString();

        expSlider.maxValue = ExperienceManager.ExperienceCap;
        expText.text = $"{ExperienceManager.Experience}/{ExperienceManager.ExperienceCap}";
    }
}
