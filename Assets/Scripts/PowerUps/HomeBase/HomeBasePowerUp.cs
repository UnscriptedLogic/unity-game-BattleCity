using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBasePowerUp : PowerUpManager
{
    public float duration = 20f;
    public GameObject emptyWall;

    protected override void Activate(Collision collision)
    {
        HomeBaseFortify homeBaseFortify = GameManager.instance.gameObject.AddComponent<HomeBaseFortify>();
        homeBaseFortify.duration = duration;
        homeBaseFortify.emptyWall = emptyWall;
        homeBaseFortify.Reconstruct(GameManager.instance);

        base.Activate(collision);
        Destroy(gameObject);
    }
}
