using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : PowerUpManager
{
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
        List<TankManager> collectorEnemies = new List<TankManager>();
        collectorEnemies = TeamManager.instance.GetEntitesNotInTeam(collectorIndexer.teamIndex);

        for (int i = 0; i < collectorEnemies.Count; i++)
        {
            collectorEnemies[i].healthScript.TakeDamage(999, collectorManager);
        }

        base.Activate(collision);
        SelfDestruct();
    }
}