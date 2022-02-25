using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TankEntitiesManager
{
    public static List<TankManager> tanks = new List<TankManager>();

    public static void AddTank(TankManager tankManager)
    {
        tanks.Add(tankManager);
    }

    public static void RemoveTank(TankManager manager)
    {
        if (tanks.Contains(manager))
        {
            tanks.Remove(manager);
        }
    }

    public static TankManager GetTankAtIndex(int index)
    {
        if (hasTankAtIndex(index))
        {
            return tanks[index];
        }

        return null;
    }

    public static bool hasTankAtIndex(int index)
    {
        if (index < tanks.Count)
        {
            return tanks.Contains(tanks[index]);
        }

        return false;
    }

    public static T GetTankComponent<T>(int index)
    {
        if (hasTankAtIndex(index))
        {
            return tanks[index].GetComponent<T>();
        }
        return default(T);
    }
}