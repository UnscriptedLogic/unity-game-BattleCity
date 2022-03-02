using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntAirborne : ShootBehaviour
{
    private List<GameObject> airborneBullets = new List<GameObject>();
    private Transform shootAnchor;
    private TankManager tankManager;
    private Transform transform;
    private int airborneAllowed;

    public IntAirborne(TankManager tankManager, Transform shootAnchor, Transform transform, int airborneAllowed)
    {
        this.tankManager = tankManager;
        this.shootAnchor = shootAnchor;
        this.transform = transform;
        this.airborneAllowed = airborneAllowed;
    }

    public override void Shoot()
    {
        for (int i = 0; i < airborneBullets.Count; i++)
        {
            if (airborneBullets[i] == null)
            {
                airborneBullets.RemoveAt(i);
            }
        }

        if (airborneBullets.Count < airborneAllowed)
        {
            BulletDetails details = new BulletDetails(
                tankManager.bulletSpeed,
                tankManager.bulletLifetime,
                transform.GetComponent<TankTeamIndexer>().teamIndex,
                tankManager.bulletHealth,
                tankManager
                );
            airborneBullets.Add(CreateBullet(tankManager.bulletPrefab, shootAnchor, details, out BulletManager bulletScript));
        }
    }
}