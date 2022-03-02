using System.Collections;
using UnityEngine;

public class BulletHealth : EntityHealth
{
    private BulletManager bulletManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        bulletManager = manager as BulletManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        EntityManager entityManager = other.GetComponent<EntityManager>();
        if (entityManager)
        {
            DamageManager.DealDamage(bulletManager.health, entityManager, bulletManager);
            return;
        }

        KillEntity();
    }
}