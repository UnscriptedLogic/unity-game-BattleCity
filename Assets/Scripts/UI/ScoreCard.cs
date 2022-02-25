using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCard : MonoBehaviour
{
    public int entityIndex;

    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI scoreTMP;

    public void SetScore(int score)
    {
        scoreTMP.text = score.ToString();
    }

    public void SetName(string entityName)
    {
        nameTMP.text = entityName;
    }
}