using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePowerUp : PowerUpManager
{
    public float duration;

    protected override void Activate(Collision collision)
    {
        Invincible invincible = collectorManager.gameObject.AddComponent<Invincible>();
        invincible.Activate(duration);

        //Sound effects and destroy
        base.Activate(collision);
        SelfDestruct();
    }
}
