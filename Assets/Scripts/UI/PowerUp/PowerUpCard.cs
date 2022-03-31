using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpCard : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI descriptionTMP;
    public GameObject stackParent;
    public TextMeshProUGUI stackTMP;
    public int stackAmount;

    public void UpdateCard(Sprite _icon, Color color, string description, int stack)
    {
        icon.sprite = _icon;
        icon.color = color;
        descriptionTMP.text = description;
        stackAmount = stack;

        if (stackAmount > 1)
        {
            stackTMP.text = stackAmount.ToString();
            stackParent.SetActive(true);
        } else
        {
            stackParent.SetActive(false);   
        }
    }

    public void ModifyStack(int amount)
    {
        stackAmount += amount;
        if (stackAmount > 1)
        {
            stackTMP.text = stackAmount.ToString();
            stackParent.SetActive(true);
        }
        else
        {
            stackParent.SetActive(false);
        }
    }
}
