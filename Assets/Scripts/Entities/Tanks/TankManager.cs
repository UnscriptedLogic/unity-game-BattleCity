using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : EntityManager
{
    [HideInInspector] public float bulletSpeed = 30f;
    [HideInInspector] public float bulletLifetime = 10f;
    [HideInInspector] public int bulletHealth = 1;
    [HideInInspector] public BulletType bulletType = BulletType.Normal;
    [HideInInspector] public GameObject bulletPrefab;
    [HideInInspector] public int tankIndex;

    public TankSettings tankSettings { get; private set; }

    public override void InitializeEntity()
    {
        base.InitializeEntity();
        tankSettings = (TankSettings)settings;
        TankIndexManager.instance.IndexMe(this);

        bulletSpeed = tankSettings.bulletSettings.speed;
        bulletLifetime = tankSettings.bulletSettings.lifetime;
        bulletPrefab = tankSettings.bulletPrefab;
        bulletHealth = tankSettings.bulletSettings.health;
        bulletType = tankSettings.bulletSettings.bulletType;
    }

    private void OnDestroy()
    {
        TankIndexManager.instance.RemoveTankIndex(tankIndex);
    }
}
