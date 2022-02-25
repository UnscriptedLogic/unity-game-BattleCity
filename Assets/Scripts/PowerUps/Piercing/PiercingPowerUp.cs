using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingPowerUp : PowerUpManager
{
    private TankManager manager;
    public int increaseAmount = 1;

    protected override void Activate(Collision collision)
    {
        PiercingPower powerScript = collision.gameObject.GetComponent<PiercingPower>();
        if (!powerScript)
        {
            powerScript = collision.gameObject.AddComponent<PiercingPower>();
        }

        powerScript.ModifyAttack(increaseAmount);
        base.Activate(collision);
        SelfDestruct();
    }
}
