using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexManager : Manager
{
    public static IndexManager instance;

    [SerializeField]
    public int startingIndex = 1;
    private int currentIndex;

    public Dictionary<int, Transform> entityIndexes = new Dictionary<int, Transform>();

    public event Action<Transform> onEntityAdded;
    public event Action<Transform> onEntiyRemoved;

    public override void Initialize()
    {
        instance = this;
        currentIndex = startingIndex - 1;

        base.Initialize();
    }

    public int SetNewEntity(Transform entityTransform)
    {
        currentIndex++;
        entityIndexes.Add(currentIndex, entityTransform);
        onEntityAdded?.Invoke(entityTransform);
        return currentIndex;
    }

    public Transform RetrieveEntity(int index)
    {
        return entityIndexes[index];
    }

    public void RemoveEntity(int index)
    {
        onEntiyRemoved?.Invoke(entityIndexes[index]);
        entityIndexes.Remove(index);
    }
}