    using System.Collections;
using UnityEngine;

public class BotHealth : TankHealth
{
    public int overrideHealth = 0;

    //public override void Initialize(EntityManager entityManager)
    //{
    //    manager.health = overrideHealth == 0 ? manager.health : overrideHealth;

    //    base.Initialize(manager);
    //}

    //public override void KillEntity()
    //{
    //    base.KillEntity();
    //}

    //protected override void OnCollisionEnter(Collision collision)
    //{
    //    base.OnCollisionEnter(collision);
    //}
}