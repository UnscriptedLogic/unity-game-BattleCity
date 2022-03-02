using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : EntityManager
{
    [Header("Tank Manager Settings")]
    public float bulletSpeed = 30f;
    public float bulletLifetime = 10f;
    public int bulletHealth = 1;
    public BulletType bulletType = BulletType.Normal;
    public GameObject bulletPrefab;
    public int tankIndex;

    private TankSettings tankSettings;

    public override void InitializeEntity()
    {
        base.InitializeEntity();
        tankSettings = (TankSettings)entitySettings;

        bulletSpeed = tankSettings.bulletSettings.speed;
        bulletLifetime = tankSettings.bulletSettings.lifetime;
        bulletPrefab = tankSettings.bulletPrefab;
        bulletHealth = tankSettings.bulletSettings.health;
        bulletType = tankSettings.bulletSettings.bulletType;
    }
}
