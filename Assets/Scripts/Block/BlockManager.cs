using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlockManager : EntityManager
{
    public BlockSettings wallSettings;
    public BlockName myWallType;
    [HideInInspector] public BlockType myWall;

    [HideInInspector] public int requiredHealth;

    public Transform colliderParent;
    public Transform gfxParent;
    public BoxCollider boxCollider;
    public BlockHealth blockHealth;

    private void Start()
    {
        InitializeBlockSettings();
    }

    public void InitializeBlockSettings()
    {
        UpdateWallSettings();
        UpdateHitRequirements();

    }

    public void ReInitializeGraphics()
    {
        for (int i = 0; i < gfxParent.childCount; i++)
        {
            Destroy(gfxParent.GetChild(0).gameObject);
        }

        GameObject graphics = Instantiate(myWall.wallGraphics, gfxParent);
        graphics.transform.localPosition = Vector3.zero;
    }

    private void UpdateWallSettings()
    {
        for (int i = 0; i < wallSettings.wallTypes.Length; i++)
        {
            if (wallSettings.wallTypes[i].wallType == myWallType)
            {
                myWall = wallSettings.wallTypes[i];
            }
        }
    }

    private void UpdateHitRequirements()
    {
        health = myWall.health;
        requiredHealth = myWall.requiredHealth;

        if (!myWall.useCollider)
        {
            boxCollider.enabled = false;
        }

        if (myWall.onlyBullets)
        {
            colliderParent.localScale = new Vector3(colliderParent.localScale.x, 0.35f, colliderParent.localScale.z);
        } else
        {
            colliderParent.localScale = new Vector3(colliderParent.localScale.x, 1f, colliderParent.localScale.z);
        }

        if (myWall.endsGameOnDeath)
        {
            DeathEndsGame deathScript = boxCollider.transform.gameObject.AddComponent<DeathEndsGame>();
            deathScript.entityHealth = deathScript.GetComponent<EntityHealth>();
        }

        return;
    }
}
