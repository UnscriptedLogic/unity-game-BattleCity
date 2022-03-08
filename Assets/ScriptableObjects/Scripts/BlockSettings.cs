using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockName
{
    Normal,
    Fortified,
    Shrub,
    Rubble,
    Base,
    Empty
}

[Serializable]
public class BlockType
{
    public BlockName wallType;
    public int health = 1;

    [Tooltip("Only projectiles strong enough can destroy this wall")]
    public int requiredHealth;

    [Tooltip("Only projectiles are allowed to pass through this object.")]
    public bool onlyBullets = false;

    [Tooltip("Disallow to let projectiles and tanks to pass through this object.")]
    public bool useCollider = true;

    [Header("Enable to end game on destroy")]
    public bool endsGameOnDeath;

    [Header("The graphics this wall should use")]
    public GameObject wallGraphics;
}

[CreateAssetMenu(fileName = "BlockSettings", menuName = "ScriptableObjects/Block Settings")]
public class BlockSettings : ScriptableObject
{
    public BlockType[] wallTypes;
}
