using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TankIndexManager : Semaphore
{
    public static TankIndexManager instance;

    [SerializeField]
    public int startingIndex = 1;
    private int currentIndex;

    public Dictionary<int, TankManager> tankIndexes = new Dictionary<int, TankManager>();
    public event Action<TankManager> onTankAdded;
    public event Action<TankManager> onTankRemoved;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);

        instance = this;
    }

    public void RemoveTankIndex(int index)
    {
        onTankRemoved?.Invoke(tankIndexes[index]);
        tankIndexes.Remove(index);
    }

    public void IndexMe(TankManager tankManager)
    {
        //Only index the tank if it does not exist already
        if (!tankIndexes.ContainsValue(tankManager))
        {
            tankManager.tankIndex = currentIndex;
            tankIndexes.Add(currentIndex, tankManager);

            currentIndex++;
            onTankAdded?.Invoke(tankManager);
        }
    }
}