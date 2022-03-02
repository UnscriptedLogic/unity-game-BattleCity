using System.Collections;
using UnityEngine;

public class BlockHealth : EntityHealth
{
    private BlockManager wallManager;
    BulletManager bmanager;

    //public override void Initialize(EntityManager entityManager)
    //{
    //    wallManager = (BlockManager)manager;

    //    base.Initialize(entityManager);
    //}

    //public override void TakeDamage(int damage, EntityManager source)
    //{
    //    TankManager tankManager = source as TankManager;
    //    if (tankManager.bulletHealth >= wallManager.requiredHealth)
    //    {
    //        base.TakeDamage(damage, source);
    //    }
    //}

    //protected override void OnCollisionEnter(Collision collision)
    //{

    //}
}