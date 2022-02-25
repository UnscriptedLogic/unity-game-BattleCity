using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePowerUp : PowerUpManager
{
    public float duration;
    public GameObject forcefieldPrefab;

    protected override void Activate(Collision collision)
    {
        GameObject forcefield = Instantiate(forcefieldPrefab, collision.transform);
        Invincible invincibleScript = forcefield.GetComponent<Invincible>();
        invincibleScript.duration = duration;
        invincibleScript.shieldOwner = collectorManager;

        base.Activate(collision);
        SelfDestruct();
    }
}
