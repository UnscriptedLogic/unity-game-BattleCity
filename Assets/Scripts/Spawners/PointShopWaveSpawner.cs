using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointShopWaveSpawner : EntitySpawnManager
{
    [Serializable]
    public class SpawnItem
    {
        public GameObject item;
        public int cost;
        public int spawnAfterWave;
        
        [Tooltip("Determines how many waves between this entity's spawns. 0 = every wave, 1 = alternate...")]
        public int waveCooldown;
        private int _waveCD;

        public bool isWaveCDDone { get => _waveCD >= waveCooldown; }
        public void IncrementCD() => _waveCD++;
        public void ResetWaveCD() => _waveCD = 0;
        public void InitializeCD() => _waveCD = waveCooldown;
    }

    public enum PointSpawnerState
    {
        Stopped,
        StartDelay,
        WaveDelay,
        Spawning,
        SpawnWait,
        MaxCapWait
    }

    [Header("Point Shop Variables")]
    [SerializeField] private int maxPoints = 3;
    [SerializeField] private int incrementPoint = 1;

    [Header("Spawn Settings")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float waveInterval = 5f;
    [SerializeField] private float waveIntervalIncrement = 0.5f;
    [SerializeField] private float waitInterval = 3f;
    [SerializeField] private float waitIntervalIncrement = 1f;
    [Space(15)]
    [SerializeField] private float checkRadius = 0.45f;
    [SerializeField] private PointSpawnerState spawnerState;

    private float _spawnInterval;
    private float _waveInterval;
    private float _waitInterval;
    private int toSpawnIndex;
    private int waveCount;

    public SpawnItem[] spawnItems;

    //Entity and their cost
    private Dictionary<GameObject, int> entityMenu = new Dictionary<GameObject, int>();
    
    //Copy of all entity gameobjects
    private List<GameObject> possibleEntities = new List<GameObject>();

    private GameObject[] entitiesToSpawn;

    public event Action onWaveStarted;
    public event Action onWaveStarting;

    public float CurrentWaveInterval { get => _waveInterval; }

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);

        //Set up the dictionary 
        possibleEntities.Clear();
        entityMenu.Clear();
        for (int i = 0; i < spawnItems.Length; i++)
        {
            possibleEntities.Add(spawnItems[i].item);
            entityMenu.Add(spawnItems[i].item, spawnItems[i].cost);
            spawnItems[i].InitializeCD();
        }

        SwitchState(PointSpawnerState.StartDelay);
    }

    private void Update()
    {
        UpdateState(spawnerState);
    }

    //Runs once when switching state
    private void EnterState(PointSpawnerState state)
    {
        switch (state)
        {
            case PointSpawnerState.Stopped:
                break;
            case PointSpawnerState.StartDelay:
                _waveInterval = startDelay;
                break;
            case PointSpawnerState.WaveDelay:
                onWaveStarting?.Invoke();
                _waveInterval = waveInterval;
                waveInterval += waveIntervalIncrement;
                toSpawnIndex = 0;

                entitiesToSpawn = GetListOfEntities();
                break;
            case PointSpawnerState.Spawning:
                waveCount++;
                for (int i = 0; i < spawnItems.Length; i++)
                {
                    spawnItems[i].IncrementCD();
                }
                break;
            case PointSpawnerState.SpawnWait:
                _waitInterval = waitInterval;
                break;
            case PointSpawnerState.MaxCapWait:
                break;
            default:
                break;
        }

        spawnerState = state;
    }

    //Runs on update
    private void UpdateState(PointSpawnerState state)
    {
        switch (state)
        {
            case PointSpawnerState.Stopped:
                break;
            case PointSpawnerState.StartDelay:
                if (_waveInterval <= 0f)
                {
                    SwitchState(PointSpawnerState.WaveDelay);
                    _waveInterval = waveInterval;
                }
                else
                {
                    _waveInterval -= Time.deltaTime;
                }
                break;
            case PointSpawnerState.WaveDelay:
                if (_waveInterval <= 0f)
                {
                    SwitchState(PointSpawnerState.Spawning);
                    _waveInterval = waveInterval;
                }
                else
                {
                    _waveInterval -= Time.deltaTime;
                }
                break;
            case PointSpawnerState.Spawning:
                if (_spawnInterval <= 0f)
                {
                    GameObject entity = SpawnEntity(entitiesToSpawn[toSpawnIndex]);
                    toSpawnIndex++;
                    _spawnInterval = spawnInterval;

                    if (toSpawnIndex >= entitiesToSpawn.Length)
                    {
                        SwitchState(PointSpawnerState.SpawnWait);
                        break;
                    }
                }
                else
                {
                    _spawnInterval -= Time.deltaTime;
                }

                if (spawnParent.childCount >= maxAlive)
                {
                    SwitchState(PointSpawnerState.MaxCapWait);
                    break;
                }
                break;
            case PointSpawnerState.SpawnWait:

                if (_waitInterval <= 0f)
                {
                    SwitchState(PointSpawnerState.WaveDelay);
                }
                else
                {
                    //Check if map is empty, do early spawn
                    if (spawnParent.childCount == 0)
                    {
                        _waitInterval = 0f;
                    }

                    _waitInterval -= Time.deltaTime;
                }
                break;
            case PointSpawnerState.MaxCapWait:
                if (spawnParent.childCount < maxAlive)
                {
                    SwitchState(PointSpawnerState.Spawning);
                }
                break;
            default:
                break;
        }
    }

    //Runs once when exiting the state
    private void ExitState(PointSpawnerState state)
    {
        switch (state)
        {
            case PointSpawnerState.Stopped:
                break;
            case PointSpawnerState.StartDelay:
                break;
            case PointSpawnerState.WaveDelay:
                onWaveStarted?.Invoke();
                break;
            case PointSpawnerState.Spawning:
                maxPoints += incrementPoint;
                break;
            case PointSpawnerState.SpawnWait:
                waitInterval += waitIntervalIncrement;
                break;
            case PointSpawnerState.MaxCapWait:
                break;
            default:
                break;
        }
    }

    //Used to transition between the states
    private void SwitchState(PointSpawnerState state)
    {
        ExitState(spawnerState);
        spawnerState = state;
        EnterState(spawnerState);
    }

    private GameObject[] GetListOfEntities()
    {
        int attempts = 100;
        int pointsToSpend = maxPoints;
        List<GameObject> potentialEntites = new List<GameObject>(possibleEntities);
        List<GameObject> confirmedEntities = new List<GameObject>();

        while (pointsToSpend > 0 && attempts > 0)
        {
            GameObject selectedEntity = RandomValue.FromList(potentialEntites.ToArray(), out int index);

            if (CanSelectEntity(index))
            {
                if (entityMenu[selectedEntity] <= pointsToSpend)
                {
                    pointsToSpend -= entityMenu[selectedEntity];
                    confirmedEntities.Add(selectedEntity);
                }
                else
                {
                    //Cut off those that are no longer in the price range
                    potentialEntites.RemoveRange(index, potentialEntites.Count - index);
                }
            } else
            {
                potentialEntites.RemoveAt(index);
            }

            attempts--;
        }
        return confirmedEntities.ToArray();
    }

    private bool CanSelectEntity(int index)
    {
        Debug.Log(spawnItems[index].spawnAfterWave);
        Debug.Log(waveCount);

        if (spawnItems[index].spawnAfterWave <= waveCount)
        {
            if (spawnItems[index].isWaveCDDone)
            {
                spawnItems[index].ResetWaveCD();
                return true;
            }
        }

        return false;
    }

    private GameObject SpawnEntity(GameObject entity)
    {
        int attempts = 50;
        while (attempts > 0)
        {
            Vector3 spawnLoc = transform.position + RandomValue.InArea(new Vector3(spawnArea.x, 0f, spawnArea.y));
            if (CheckSpawnValid(spawnLoc, blockLayer, checkRadius))
            {
                return Spawn(entity, spawnLoc);
            }

            attempts--;
        }

        Debug.Log("Could not find a place to spawn");
        SwitchState(PointSpawnerState.Stopped);
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
