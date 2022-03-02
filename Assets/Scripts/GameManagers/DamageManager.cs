using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageManager
{
    public static event Action<TankManager, TankManager> onKillEvent;
    public static event Action<int, TankManager, TankManager> onTankHitEvent;

    //The entrance. This filters out who function is called based on the victim
    public static void DealDamage(int amount, EntityManager victim, BulletManager bulletManager = null)
    {
        if (victim as TankManager)
        {
            DealDamageBetweenTanks(amount: amount, victim: victim as TankManager, culprit: bulletManager.origin, bulletManager: bulletManager);
        } else
        {
            DealDamageToEntities(amount, victim, bulletManager);
        }
    }

    //Damage dealt between blocks and tanks
    public static void DealDamageToEntities(int amount, EntityManager victim, BulletManager bulletManager = null)
    {
        int victimHealth = victim.health;
        EntityHealth entityHealth = victim.GetComponent<EntityHealth>();
        entityHealth.TakeDamage(amount);

        //In case of indirect damage
        if (bulletManager)
        {
            bulletManager.GetComponent<BulletHealth>().TakeDamage(victimHealth);
        }
    }

    //Damage done between tanks
    public static void DealDamageBetweenTanks(int amount, TankManager victim, TankManager culprit = null, BulletManager bulletManager = null)
    {
        //Friendly fire checking
        if (bulletManager.teamIndex != TeamManager.instance.GetTeamOfTank(victim))
        {
            int victimHealth = victim.health;
            EntityHealth tankHealth = victim.GetComponent<EntityHealth>();
            tankHealth.TakeDamage(amount);

            if (victim.health <= 0)
            {
                if (culprit)
                {
                    //Announce who killed who
                    onKillEvent?.Invoke(bulletManager.origin, victim); 
                }
            }

            //Sometimes damage is done indirectly. E.g. Nuke powerup
            if (bulletManager)
            {
                BulletHealth bulletHealth = bulletManager.GetComponent<BulletHealth>();
                bulletHealth.TakeDamage(victimHealth);
            }

            onTankHitEvent?.Invoke(victimHealth - victim.health, bulletManager.origin, victim);
        }
    }
}