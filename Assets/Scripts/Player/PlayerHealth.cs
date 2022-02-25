using System.Collections;
using UnityEngine;

public class PlayerHealth : TankHealth
{
    public override void KillEntity()
    {
        base.KillEntity();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}