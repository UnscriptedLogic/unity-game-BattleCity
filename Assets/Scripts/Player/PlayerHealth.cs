using System.Collections;
using UnityEngine;

public class PlayerHealth : TankHealth
{
    private PlayerManager playerManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        playerManager = manager as PlayerManager;
    }

    //public override void KillEntity()
    //{
    //    base.KillEntity();
    //}

    //protected override void OnCollisionEnter(Collision collision)
    //{
    //    base.OnCollisionEnter(collision);
    //}
}