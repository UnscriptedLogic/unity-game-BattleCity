using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockManager : BlockManager
{
    public static BaseBlockManager instance;
    private void Awake()
    {
        instance = this;
    }

    public Transform[] walls;
    [HideInInspector] public List<BlockManager> baseWallManagers = new List<BlockManager>();

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < walls.Length; i++)
        {
            baseWallManagers.Add(walls[i].GetChild(0).GetComponentInChildren<BlockManager>());
        }

        for (int i = 0; i < baseWallManagers.Count; i++)
        {
            baseWallManagers[i].GetComponent<EntityHealth>().onDeathDisables = true;
            baseWallManagers[i].GetComponent<EntityHealth>().onDeathDestroys = false;
        }
    }
}