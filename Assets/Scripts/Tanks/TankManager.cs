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
    public int tankIndex;

    public TankHealth healthScript;

    [Header("Tank Settings")]
    public TankSettings tankSettings;

    protected override void InitSettings()
    {
        tankSettings = (TankSettings)entitySettings;
    }

    protected override void Initialize()
    {
        TankEntitiesManager.AddTank(this);
        bulletSpeed = tankSettings.bulletSettings.movementSpeed;
        bulletLifetime = tankSettings.bulletSettings.lifetime;
        bulletPrefab = tankSettings.bulletPrefab;
        bulletHealth = tankSettings.bulletSettings.health;
        bulletType = tankSettings.bulletSettings.bulletType;

        if (healthScript == null)
        {
            healthScript = GetComponent<TankHealth>();
        }

        base.Initialize();
    }

    private void OnDestroy()
    {
        TankEntitiesManager.RemoveTank(this);
    }
}
