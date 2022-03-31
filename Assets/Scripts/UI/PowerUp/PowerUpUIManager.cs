using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PowerUpUIDetails
{
    public PowerUpType powerUpType;
    public Sprite icon;
    public Color color = Color.white;
    [TextArea(5, 10)]
    public string description;
    public bool lostOnDeath;
    public int stackCount;
}

public class PowerUpUIManager : Semaphore
{
    public PowerUpUIDetails[] powerUpUIDetails;
    public Dictionary<PowerUpType, PowerUpUIDetails> powerUpDetails = new Dictionary<PowerUpType, PowerUpUIDetails>();
    public Dictionary<PowerUpType, PowerUpCard> powerUpCards = new Dictionary<PowerUpType, PowerUpCard>();
    public Transform uiParent;
    public GameObject uiPrefab;

    public static PowerUpUIManager instance;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        instance = this;
        for (int i = 0; i < powerUpUIDetails.Length; i++)
        {
            powerUpDetails.Add(powerUpUIDetails[i].powerUpType, powerUpUIDetails[i]);
        }
    }

    public void AddCard(PowerUpType powerUpType)
    {
        if (powerUpCards.ContainsKey(powerUpType))
        {
            ModifyStack(powerUpType, 1);
            return;
        }

        GameObject uiCard = Instantiate(uiPrefab, uiParent);
        PowerUpCard powerUpCard = uiCard.GetComponent<PowerUpCard>();

        PowerUpUIDetails details = powerUpDetails[powerUpType];
        powerUpCard.UpdateCard(details.icon, details.color, details.description, details.stackCount);

        powerUpCards.Add(powerUpType, powerUpCard);
        ModifyStack(powerUpType, 1);
    }

    public void ModifyStack(PowerUpType powerUpType, int amount)
    {
        powerUpCards[powerUpType].ModifyStack(amount);
    }

    public IEnumerator RemoveAfterDuration(PowerUpType type, float duration)
    {
        yield return new WaitForSeconds(duration);

        RemoveCard(type);
    }

    public void RemoveCard(PowerUpType type)
    {
        if (!powerUpCards.ContainsKey(type))
        {
            return;
        }

        Destroy(powerUpCards[type].gameObject);
        powerUpCards.Remove(type);
    }

    public void RemoveOnDeath()
    {
        for (int i = 0; i < powerUpUIDetails.Length; i++)
        {
            if (powerUpUIDetails[i].lostOnDeath)
            {
                RemoveCard(powerUpUIDetails[i].powerUpType);
            }
        }
    }
}