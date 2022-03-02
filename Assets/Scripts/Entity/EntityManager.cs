using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class EntityManager : Manager
{
    public EntitySettings entitySettings;

    [Header("Initialized from ScriptableObject")]
    public int health;
    public float speed;
    public float rotationSpeed;
    public int damage;

    protected virtual void Start()
    {
        InitializeSephamores();
    }

    public virtual void InitializeEntity()
    {
        health = entitySettings.health;
        speed = entitySettings.speed;
        rotationSpeed = entitySettings.rotationSpeed;
        damage = entitySettings.damage;
    }
}