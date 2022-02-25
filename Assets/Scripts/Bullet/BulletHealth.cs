using System.Collections;
using UnityEngine;

public class BulletHealth : EntityHealth
{
    private BulletManager bulletManager;
    public int originalHealth;

    public override void Initialize(EntityManager entityManager)
    {
        bulletManager = (BulletManager)entityManager;
        originalHealth = bulletManager.health;

        entityManager.onInitialized += EntityManager_onInitialized;
        base.Initialize(entityManager);
    }

    private void EntityManager_onInitialized()
    {
        Destroy(gameObject, bulletManager.lifetime);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        EntityHealth victimHealthScript = collision.transform.GetComponent<EntityHealth>();
        if (victimHealthScript)
        {
            //amount of damage, who was the culprit, who as the victim, how it was done
            DamageManager.EntityDamage(bulletManager.health, source: bulletManager.origin.healthScript, victim: victimHealthScript, medium: bulletManager.healthScript, bulletManager.teamIndex);
            return;
        }

        Destroy(gameObject);
    }
}