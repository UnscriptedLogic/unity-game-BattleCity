using System.Collections;
using UnityEngine;

public class IntervalShoot : ShootBehaviour
{
    private TankManager tankManager;
    private Transform transform;
    private Transform shootAnchor;

    private float _interval = 1f;

    public IntervalShoot(TankManager tankManager, Transform transform, Transform shootAnchor)
    {
        this.tankManager = tankManager;
        this.transform = transform;
        this.shootAnchor = shootAnchor;
    }

    public override void Shoot()
    {
        if (_interval <= 0f)
        {
            BulletDetails details = new BulletDetails(
                tankManager.bulletSpeed,
                tankManager.bulletLifetime,
                transform.GetComponent<TankTeamIndexer>().teamIndex,
                tankManager.bulletHealth,
                tankManager
                );

            CreateBullet(tankManager.bulletPrefab, shootAnchor, details, out BulletManager bulletScript);
        }
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