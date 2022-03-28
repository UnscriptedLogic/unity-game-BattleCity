using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : EntitySpawnManager
{
    [Serializable]
    public class WaveSegment
    {
        public GameObject enemyToSpawn;
        public int amount;
        public float interval;

        [HideInInspector] public int _spawnAmount = 0;
        [HideInInspector] public float _spawnInterval = 0;

        public void ContinueSpawn()
        {
            _spawnInterval = interval;
            _spawnAmount++;
        }
    }

    [Serializable]
    public class Wave
    {
        public WaveSegment[] waveSegments;
        public float waveInterval = 5f;
        public float segmentInterval = 3f;

        [HideInInspector] public float _waveInterval;
        [HideInInspector] public float _segmentInterval;
    }

    public float startDelay = 5f;
    private float _startDelay;

    public Wave[] waves;
    private Wave currWave;
    private WaveSegment currSegment;

    private int waveIndex;
    private int segmentIndex;

    private bool stopSpawning;
    private bool checkClearStage;

    public event Action onSegmentCompleted;
    public event Action onWaveCompleted;
    public event Action onAllWavesCompleted;
    public event Action onStageCleared;

    public override void Start()
    {
        _startDelay = startDelay;
        currWave = waves[waveIndex];
        currSegment = currWave.waveSegments[segmentIndex];
    }

    private void Update()
    {
        if (stopSpawning)
        {
            return;
        }

        if (_startDelay >= 0f)
        {
            _startDelay -= Time.deltaTime;
            return;
        }

        if (currWave._waveInterval <= 0f)
        {
            if (currWave._segmentInterval <= 0)
            {
                SpawnSegment();
            }
            else
            {
                currWave._segmentInterval -= Time.deltaTime;
            }
        }
        else
        {
            currWave._waveInterval -= Time.deltaTime;
        }
    }

    private void SpawnSegment()
    {
        if (currSegment._spawnInterval <= 0)
        {
            SpawnEnemy();

            if (currSegment._spawnAmount == currSegment.amount - 1)
            {
                NextWaveSegment();
            }
            else
            {
                currSegment.ContinueSpawn();
            }
        }
        else
        {
            currSegment._spawnInterval -= Time.deltaTime;
        }
    }

    private void NextWaveSegment()
    {
        segmentIndex++;
        if (segmentIndex >= currWave.waveSegments.Length)
        {
            segmentIndex = 0;
            NextWave();
            return;
        }

        currWave._segmentInterval = currWave.segmentInterval;
        currSegment = currWave.waveSegments[segmentIndex];
        onSegmentCompleted?.Invoke();
    }

    private void NextWave()
    {
        waveIndex++;
        if (waveIndex >= waves.Length)
        {
            Debug.Log("All waves completed");
            onAllWavesCompleted?.Invoke();
            stopSpawning = true;
            return;
        }

        currWave = waves[waveIndex];
        currWave._waveInterval = currWave.waveInterval;
        currSegment = currWave.waveSegments[segmentIndex];
        onWaveCompleted?.Invoke();
    }

    private void SpawnEnemy()
    {
        int counter = 50;
        Vector3 pos = transform.position + RandomValue.InArea(new Vector3(spawnArea.x, 0f, spawnArea.y));
        while (!CheckSpawnValid(pos, blockLayer, 0.45f) && counter >= 0)
        {
            pos = transform.position + RandomValue.InArea(new Vector3(spawnArea.x, 0f, spawnArea.y));
            counter--;
        }

        if (counter >= 0)
        {
            GameObject enemy = Instantiate(currSegment.enemyToSpawn, pos, Quaternion.identity, spawnParent);
            //GameObject enemy = PoolManager.instance.PullFromPool(currSegment.enemyToSpawn);
            enemy.transform.SetParent(transform);
        } else
        {
            Debug.Log("Unable to find a valid spot to spawn.");
        }
    }
}
