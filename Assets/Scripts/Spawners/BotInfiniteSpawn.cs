using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesoliteTanks.EntitySpawner
{
    [System.Serializable]
    public class BotSpawn
    {
        public GameObject prefab;
        public EntitySettings settings;
        [Range(0f, 100f)]
        public float chance;
    }

    public class BotInfiniteSpawn : EntitySpawnManager
    {
        [Header("Infinite Spawn Settings")]
        public BotSpawn[] entitySpawns;

        public float startDelay = 5f;
        public float waveInterval = 10f;
        public float spawnInterval = 1f;
        private float _interval;

        public int maxSpawn = 5;
        private int spawnCounter;

        public EntitySpawnModifiers[] spawnModifiers;

        private List<EntitySpawnModifiers> onSpawnModify = new List<EntitySpawnModifiers>();
        private List<EntitySpawnModifiers> onWaveModify = new List<EntitySpawnModifiers>();

        public event Action onWaveCompleted;
        public event Action onSpawnEntity;

        protected override void SephamoreStart(Manager manager)
        {
            base.SephamoreStart(manager);
            _interval = startDelay;
            onWaveCompleted += ModifyAfterWave;
            onSpawnEntity += ModifyAfterSpawn;

            for (int i = 0; i < spawnModifiers.Length; i++)
            {
                if (spawnModifiers[i].modifyAfterSpawn)
                {
                    onSpawnModify.Add(spawnModifiers[i]);
                }
                else
                {
                    onWaveModify.Add(spawnModifiers[i]);
                }
            }
        }

        private void Update()
        {
            //Prevents the spawner from filling up the map
            if (HasReachedEntityCap())
            {
                return;
            }

            if (_interval <= 0)
            {
                //Skips the whole loop and tries again if a invalid position is found.
                Vector3 pos = transform.position + RandomValue.InArea(new Vector3(spawnArea.x, 0f, spawnArea.y));
                if (!CheckSpawnValid(pos, blockLayer, 0.45f))
                {
                    return;
                }

                //Randomizes the entity to spawn
                int index = RandomIndex();
                GameObject entity = Spawn(entitySpawns[index].prefab, pos);
                entity.GetComponent<EntityManager>().settings = entitySpawns[index].settings;
                spawnCounter++;

                //We have reached the amount needed to spawn for this wave. Proceed to next wave
                if (spawnCounter == maxSpawn)
                {
                    _interval = waveInterval;
                    spawnCounter = 0;
                    onWaveCompleted?.Invoke();
                }
                else
                {
                    _interval = spawnInterval;
                    onSpawnEntity?.Invoke();
                }
            }
            else
            {
                _interval -= Time.deltaTime;
            }
        }

        public void ModifyAfterWave()
        {
            ModifySettings(onWaveModify);
        }

        public void ModifyAfterSpawn()
        {
            ModifySettings(onSpawnModify);
        }

        //Reduces the spawner's stats for every entity spawned or after every n entites have spawned
        public void ModifySettings(List<EntitySpawnModifiers> _modifiers)
        {
            for (int i = 0; i < _modifiers.Count; i++)
            {
                _modifiers[i].counter++;
                if (_modifiers[i].counter == _modifiers[i].toComplete)
                {
                    switch (_modifiers[i].spawnSetting)
                    {
                        case SpawnSetting.SpawnCap:
                            maxSpawn += (int)_modifiers[i].amount;
                            maxSpawn = (int)Mathf.Clamp(maxSpawn, _modifiers[i].clampValues.x, _modifiers[i].clampValues.y);
                            break;
                        case SpawnSetting.SpawnInterval:
                            spawnInterval += _modifiers[i].amount;
                            spawnInterval = Mathf.Clamp(spawnInterval, _modifiers[i].clampValues.x, _modifiers[i].clampValues.y);
                            break;
                        case SpawnSetting.WaveInterval:
                            waveInterval += _modifiers[i].amount;
                            waveInterval = Mathf.Clamp(waveInterval, _modifiers[i].clampValues.x, _modifiers[i].clampValues.y);
                            break;
                        default:
                            break;
                    }

                    _modifiers[i].counter = 0;
                }
            }
        }

        protected int RandomIndex()
        {
            float[] tierChances = new float[entitySpawns.Length];
            float prevChance = 0f;
            for (int i = 0; i < entitySpawns.Length; i++)
            {
                tierChances[i] = prevChance + entitySpawns[i].chance;
                prevChance = tierChances[i];
            }

            int randomTier = UnityEngine.Random.Range(0, 100);
            for (int i = 0; i < tierChances.Length; i++)
            {
                float highNum = i == tierChances.Length - 1 ? 100 : tierChances[i];
                float lowNum = i == 0 ? 0 : tierChances[i - 1];
                if (randomTier > lowNum && randomTier < highNum)
                {
                    return i;
                }
            }

            return 0;
        }

        private void OnValidate()
        {
            float totalChance = 100f;
            for (int i = 1; i < entitySpawns.Length; i++)
            {
                if (entitySpawns[i].chance > totalChance - entitySpawns[i - 1].chance)
                {
                    entitySpawns[i].chance = totalChance - entitySpawns[i - 1].chance;
                }
                totalChance -= entitySpawns[i - 1].chance;
            }
        }
    } 
}