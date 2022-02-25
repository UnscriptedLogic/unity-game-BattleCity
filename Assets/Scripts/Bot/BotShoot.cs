using System.Collections;
using UnityEngine;

public class BotShoot : EntityShoot
{
    public BotManager manager;
    public Transform shootAnchor;
    private float _shootInterval = 999f;

    public override void Initialize(EntityManager entityManager)
    {
        _shootInterval = 0f;
        base.Initialize(entityManager);
    }

    private void Update()
    {
        if (_shootInterval <= 0f)
        {
            BulletDetails details = new BulletDetails(
                manager.enemySettings.bulletSettings, 
                manager.bulletSpeed, 
                manager.bulletLifetime, 
                transform.GetComponent<TankTeamIndexer>().teamIndex, 
                manager.bulletHealth,
                manager
                );
            CreateBullet(manager.bulletPrefab, shootAnchor, details, out BulletManager bulletScript);
            _shootInterval = RandomValue.BetweenFloats(manager.shootIntervals.x, manager.shootIntervals.y);
        } else
        {
            _shootInterval -= Time.deltaTime;
        }
    }
}