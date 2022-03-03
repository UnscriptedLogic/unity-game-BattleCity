using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : PowerUpManager
{
    public int explosionDamage = 25;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Activate(Collision collision)
    {
        TankTeamIndexer collectorIndexer = collision.transform.GetComponent<TankTeamIndexer>();
        List<TankManager> collectorEnemies = new List<TankManager>(TeamManager.instance.GetTanksNotInTeam(collectorIndexer.teamIndex));

        for (int i = 0; i < collectorEnemies.Count; i++)
        {
            if (collectorEnemies[i] != null)
            {
                DamageManager.DealDamageBetweenTanks(amount: explosionDamage, victim: collectorEnemies[i], culpritTankIndex: collectorManager.tankIndex);
            }
        }

        base.Activate(collision);
        SelfDestruct();
    }
}