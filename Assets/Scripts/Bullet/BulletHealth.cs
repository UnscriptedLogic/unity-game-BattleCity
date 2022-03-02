using System.Collections;
using UnityEngine;

public class BulletHealth : EntityHealth
{
    private BulletManager bulletManager;
    private int originalHealth;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        bulletManager = manager as BulletManager;
        originalHealth = bulletManager.health;
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
        //As of right now there are 3 things in the world that can take damage
        //- Blocks (amount)
        //- Tanks (amount, Team Index)
        //- Bullets (amount, Team Index) => problem here when 2 entities are trying to damage each other

        //The problem is trying to call one function and let the rest handle it.
        //And returning the damage back to know how much health the bullet has left
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    EntityManager entityManager = collision.transform.GetComponent<EntityManager>();
    //    if (entityManager)
    //    {
    //        DamageManager.DealDamage(bulletManager.health, entityManager, bulletManager);
    //        return;
    //    }

    //    KillEntity();

    //}
}