using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUI : Semaphore
{
    public EntityHealth health;
    private PowerUpUIManager uiManager;
    private PowerUpManager powerUpManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        health.onKilled += Health_onKilled;
        uiManager = PowerUpUIManager.instance;
    }

    private void Health_onKilled()
    {
        uiManager.RemoveOnDeath();
    }

    private void OnCollisionEnter(Collision collision)
    {
        powerUpManager = collision.transform.GetComponent<PowerUpManager>();
        if (uiManager && powerUpManager)
        {
            uiManager.AddCard(powerUpManager.powerUpType);
            switch (powerUpManager.powerUpType)
            {
                case PowerUpType.ArmorPiercing:
                    break;
                case PowerUpType.Invincible:
                    InvinciblePowerUp invincible = powerUpManager as InvinciblePowerUp;
                    StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.Invincible, invincible.duration));
                    break;
                case PowerUpType.Time:
                    StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.Time, 10));
                    break;
                case PowerUpType.Nuke:
                    StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.Nuke, 3));
                    break;
                case PowerUpType.HomeFortify:
                    StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.HomeFortify, 15));
                    break;
                default:
                    break;
            }
        }
    }
}
