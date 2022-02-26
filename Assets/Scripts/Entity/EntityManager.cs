using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class EntityManager : MonoBehaviour
{
    public EntitySettings entitySettings;

    [HideInInspector] public int health;
    [HideInInspector] public float movementSpeed = 10f;
    [HideInInspector] public float rotationSpeed = 30f;

    private SemaphoreSlim gate = new SemaphoreSlim(1);
    public EntitySemaphore[] entityScripts;
    public event Action onInitialized;

    protected virtual void OnEnable()
    {
        InitSettings();
        Initialize();

        InitializeScripts();
    }

    protected async void InitializeScripts()
    {
        for (int i = 0; i < entityScripts.Length; i++)
        {
            await gate.WaitAsync();
            entityScripts[i].Initialize(this);
        }

        onInitialized?.Invoke();
    }

    public void ReleaseGate(EntitySemaphore script)
    {
        gate.Release();
    }

    protected virtual void InitSettings()
    {

    }

    protected virtual void Initialize()
    {
        health = entitySettings.health;
        movementSpeed = entitySettings.movementSpeed;
        rotationSpeed = entitySettings.rotationSpeed;
    }
}