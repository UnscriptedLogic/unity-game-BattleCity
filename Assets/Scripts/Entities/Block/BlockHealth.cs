using System.Collections;
using UnityEngine;

public class BlockHealth : EntityHealth
{
    private BlockManager blockManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        blockManager = manager as BlockManager;
    }

    public void BlockTakeDamage(int amount, int originalBulletHealth)
    {
        if (originalBulletHealth >= blockManager.requiredHealth)
        {
            TakeDamage(amount);
        }
    }

    //public override void TakeDamage(int damage)
    //{
    //    base.TakeDamage(damage);
    //}

    //public override void TakeDamage(int damage, EntityManager source)
    //{
    //    TankManager tankManager = source as TankManager;
    //    if (tankManager.bulletHealth >= blockManager.requiredHealth)
    //    {
    //        base.TakeDamage(damage, source);
    //    }
    //}
}