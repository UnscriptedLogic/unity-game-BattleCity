using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePowerUp : PowerUpManager
{
    public float duration;
    public GameObject forcefieldPrefab;
    private BoxCollider boxCollider;

    protected override void Activate(Collision collision)
    {
        boxCollider = collectorManager.GetComponent<BoxCollider>();

        //Create a gameobject to house the fake collider
        GameObject fakeColliderGO = Instantiate(forcefieldPrefab, collectorManager.transform);
        fakeColliderGO.transform.position = collectorManager.transform.position;
        fakeColliderGO.transform.localScale = Vector3.one;

        //Create a fake collider - damage scripts will look for an entityhealth. Seeing there's none, no damage done
        BoxCollider fakeBoxCollider = fakeColliderGO.AddComponent<BoxCollider>();
        fakeBoxCollider.size = boxCollider.size;
        fakeBoxCollider.center = boxCollider.center;
        fakeBoxCollider.isTrigger = false;

        //Adds the duration script and what to do when the script duration runs out
        Invincible invincible = collectorManager.gameObject.AddComponent<Invincible>();
        invincible.fakeColliderGO = fakeColliderGO;
        invincible.duration = duration;
        invincible.realCollder = boxCollider;

        //Sound effects and destroy
        base.Activate(collision);
        SelfDestruct();
    }
}
