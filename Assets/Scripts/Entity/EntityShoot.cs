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
    public BulletSettings bulletSettings;

    public BulletDetails(BulletSettings bulletSettings, float speed, float lifetime, int team, int health, TankManager origin)
    {
        this.bulletSettings = bulletSettings;
        this.speed = speed;
        this.lifetime = lifetime;
        this.team = team;
        this.health = health;
        this.origin = origin;
    }
}

public class EntityShoot : EntitySemaphore
{
    public event Action<GameObject> onBulletCreated;

    protected GameObject CreateBullet(GameObject prefab, Transform _shootAnchor, BulletDetails details, out BulletManager bulletScript)
    {
        GameObject bullet = Instantiate(prefab, _shootAnchor.position, _shootAnchor.rotation);

        bulletScript = bullet.GetComponent<BulletManager>();
        bulletScript.entitySettings = details.bulletSettings;
        bulletScript.speed = details.speed;
        bulletScript.lifetime = details.lifetime;
        bulletScript.teamIndex = details.team;
        bulletScript.health = details.health;
        bulletScript.origin = details.origin;
        //bulletScript.SettingsInitialized();

        onBulletCreated?.Invoke(bullet);
        return bullet;
    }
}