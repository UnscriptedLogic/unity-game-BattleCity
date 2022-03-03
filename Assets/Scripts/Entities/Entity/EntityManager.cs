using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class EntityManager : Manager
{
    public EntitySettings settings;

    [HideInInspector] public int health;
    [HideInInspector] public float speed;
    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public int damage;

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