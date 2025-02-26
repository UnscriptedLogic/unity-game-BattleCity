﻿using System.Collections;
using UnityEngine;

public class IntervalShoot : ShootBehaviour
{
    private TankManager tankManager;
    private Transform transform;
    private EntityShoot entityShoot;

    private float _interval = 1f;

    public IntervalShoot(TankManager tankManager, EntityShoot entityShoot, Transform transform)
    {
        this.tankManager = tankManager;
        this.transform = transform;
        this.entityShoot = entityShoot;
    }

    public override void Shoot()
    {
        if (_interval <= 0f)
        {
            ShootBullet();
        }
    }

    public void ShootBullet()
    {
        BulletDetails details = new BulletDetails(
            tankManager.bulletSpeed,
            tankManager.bulletLifetime,
            transform.GetComponent<TankTeamIndexer>().teamIndex,
            tankManager.damage,
            tankManager,
            tankManager.tankSettings.bulletSettings
            );

        CreateBullet(tankManager.bulletPrefab, entityShoot.shootAnchor, details, out BulletManager bulletScript);
    }

    public void ResetInterval(float duration)
    {
        _interval = duration;
    }

    public void IntervalUpdate()
    {
        if (_interval >= 0f)
        {
            _interval -= Time.deltaTime;
        }
    }
}