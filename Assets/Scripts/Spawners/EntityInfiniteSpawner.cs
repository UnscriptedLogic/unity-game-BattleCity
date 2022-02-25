using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityChances
{
    public GameObject prefab;
    [Range(0f, 100f)] public int chance;
}

public class EntityInfiniteSpawner : EntitySpawnManager
{
    public EntityChances[] entityChances;

    public float yOffset;
    public float startDelay;
    private float _interval;
    public event Action onSpawnEntity;

    public EntitySpawnModifiers[] spawnModifiers;

    private List<EntitySpawnModifiers> onSpawnModify = new List<EntitySpawnModifiers>();
    public float spawnInterval;

    public override void Initialize()
    {
        _interval = startDelay;
        onSpawnEntity += ModifyAfterSpawn;

        for (int i = 0; i < spawnModifiers.Length; i++)
        {
            if (spawnModifiers[i].modifyAfterSpawn)
            {
                onSpawnModify.Add(spawnModifiers[i]);
            }
        }

        base.Initialize();
    }

    private void Update()
    {
        if (_interval <= 0f)
        {
            Vector3 pos = transform.position + RandomValue.InArea(new Vector3(spawnArea.x, 0f, spawnArea.y));
            if (!CheckSpawnValid(pos, 0.45f) || HasReachedEntityCap())
            {
                return;
            }

            //int index = RandomIndex();
            float[] chances = new float[entityChances.Length];
            for (int i = 0; i < chances.Length; i++)
            {
                chances[i] = entityChances[i].chance;
            }
            int index = RandomValue.RandomIndex(entityChances, chances);
            GameObject entity = Spawn(entityChances[index].prefab, new Vector3(pos.x, yOffset,pos.z));

            _interval = spawnInterval;
            onSpawnEntity?.Invoke();
        }
        else
        {
            _interval -= Time.deltaTime;
        }
    }

    public void ModifyAfterSpawn()
    {
        ModifySettings(onSpawnModify);
    }

    public void ModifySettings(List<EntitySpawnModifiers> _modifiers)
    {
        for (int i = 0; i < _modifiers.Count; i++)
        {
            _modifiers[i].counter++;
            if (_modifiers[i].counter == _modifiers[i].toComplete)
            {
                switch (_modifiers[i].spawnSetting)
                {
                    case SpawnSetting.SpawnInterval:
                        spawnInterval += _modifiers[i].amount;
                        spawnInterval = Mathf.Clamp(spawnInterval, _modifiers[i].clampValues.x, _modifiers[i].clampValues.y);
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
        float[] tierChances = new float[entityChances.Length];
        float prevChance = 0f;
        for (int i = 0; i < entityChances.Length; i++)
        {
            tierChances[i] = prevChance + entityChances[i].chance;
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
        for (int i = 1; i < entityChances.Length; i++)
        {
            if (entityChances[i].chance > totalChance - entityChances[i - 1].chance)
            {
                entityChances[i].chance = (int)totalChance - entityChances[i - 1].chance;
            }
            totalChance -= entityChances[i - 1].chance;
        }
    }
}