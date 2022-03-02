using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUI : EntitySemaphore
{
    public TankHealth health;
    private PowerUpUIManager uiManager;
    private PowerUpManager powerUpManager;

    //public override void Initialize(EntityManager manager)
    //{
    //    health.onKilled += Health_onKilled;
    //    uiManager = PowerUpUIManager.instance;

    //    base.Initialize(manager);
    //}

    //private void Health_onKilled(EntityManager obj)
    //{
    //    uiManager.RemoveOnDeath();
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    powerUpManager = collision.transform.GetComponent<PowerUpManager>();
    //    if (uiManager && powerUpManager)
    //    {
    //        uiManager.AddCard(powerUpManager.powerUpType);
    //        switch (powerUpManager.powerUpType)
    //        {
    //            case PowerUpType.ArmorPiercing:
    //                break;
    //            case PowerUpType.Invincible:
    //                InvinciblePowerUp invincible = powerUpManager as InvinciblePowerUp;
    //                StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.Invincible, invincible.duration));
    //                break;
    //            case PowerUpType.Time:
    //                TimePowerUp time = powerUpManager as TimePowerUp;
    //                time.onTimeAffected += Time_onTimeAffected;
    //                break;
    //            case PowerUpType.Nuke:
    //                StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.Nuke, 3));
    //                break;
    //            case PowerUpType.HomeFortify:
    //                StartCoroutine(uiManager.RemoveAfterDuration(PowerUpType.HomeFortify, 10));
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //}

    //private void Time_onTimeAffected(TimeManager obj)
    //{
    //    obj.onTimeResumed += Time_onTimeResumed;
    //}

    //private void Time_onTimeResumed()
    //{
    //    uiManager.RemoveCard(PowerUpType.Time);
    //}
}
