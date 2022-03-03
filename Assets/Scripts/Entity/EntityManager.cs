using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class EntityManager : Manager
{
    public EntitySettings settings;

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
        health = settings.health;
        speed = settings.speed;
        rotationSpeed = settings.rotationSpeed;
        damage = settings.damage;
    }
}