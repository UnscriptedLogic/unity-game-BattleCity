using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnSetting
{
    SpawnCap,
    SpawnInterval,
    WaveInterval
}

[System.Serializable]
public class EntitySpawnModifiers
{
    public SpawnSetting spawnSetting;
    public int toComplete = 2;
    public float amount;
    [Tooltip("Check if you want the effects to modify after every spawn instead. Disable to modify after wave.")]
    public bool modifyAfterSpawn;
    public Vector2 clampValues;

    [HideInInspector] public int counter;
}


public class EntitySpawnManager : Semaphore
{
    private GameManager gameManager;

    [Header("Manager Settings")]
    public Vector2 spawnArea = new Vector2(2f, 2f);
    public Transform spawnParent;

    [Tooltip("The spawner will stop spawning when it has reached this limit of entities alive on the field. Set zero to infinite")]
    public int maxAlive = 0;

    public Component[] addOnSpawn;
    public event Action<GameObject> onSpawn;
    public event Action<int> onReachedEntityCap;

    public bool canSpawn = false;

    public virtual void Start()
    {
        
    }

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        gameManager = manager as GameManager;

        gameManager.onGameStarted += OnGameInitialized;
    }

    protected void OnGameInitialized()
    {
        canSpawn = true;
    }

    protected GameObject Spawn(GameObject prefab, Vector3 position)
    {
        GameObject entity = Instantiate(prefab, position, Quaternion.identity, spawnParent);

        for (int i = 0; i < addOnSpawn.Length; i++)
        {
            entity.AddComponent(addOnSpawn[i].GetType());
        }

        onSpawn?.Invoke(entity);
        return entity;
    }

    protected bool CheckSpawnValid(Vector3 position, float checkRadius = 0.25f)
    {
        return !Physics.CheckSphere(position, checkRadius);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 0f, spawnArea.y));
    }

    protected bool HasReachedEntityCap()
    {
        if (maxAlive > 0)
        {
            if (spawnParent.childCount >= maxAlive)
            {
                onReachedEntityCap?.Invoke(maxAlive);
            }

            return spawnParent.childCount >= maxAlive;
        }

        return false;
    }
}
