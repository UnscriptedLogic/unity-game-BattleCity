using System;
using System.Collections;
using UnityEngine;

public class BulletDetails
{
    public float speed;
    public float lifetime;
    public int team;
    public int health;
    public TankManager origin;
    public BulletSettings bulletEffects;

    public BulletDetails(float speed, float lifetime, int team, int health, TankManager origin, BulletSettings bulletEffects)
    {
        this.speed = speed;
        this.lifetime = lifetime;
        this.team = team;
        this.health = health;
        this.origin = origin;
        this.bulletEffects = bulletEffects;
    }
}

public class ShootBehaviour
{
    public event Action onBulletShot;

    public virtual void Shoot()
    {

    }

    protected GameObject CreateBullet(GameObject prefab, Transform _shootAnchor, BulletDetails details, out BulletManager bulletScript)
    {
        GameObject bullet = UnityEngine.Object.Instantiate(prefab, _shootAnchor.position, _shootAnchor.rotation);

        bulletScript = bullet.GetComponent<BulletManager>();
        bulletScript.speed = details.speed;
        bulletScript.lifetime = details.lifetime;
        bulletScript.teamIndex = details.team;
        bulletScript.health = details.health;
        bulletScript.origin = details.origin;
        bulletScript.settings = details.bulletEffects;
        onBulletShot?.Invoke();

        return bullet;
    }
}