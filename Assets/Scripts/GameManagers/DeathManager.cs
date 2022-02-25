using System;
using System.Collections;
using UnityEngine;

public class DeathManager : Manager
{
    public event Action<int, int> onKilled;

    public static DeathManager instance;

    public override void Initialize()
    {
        instance = this;

        base.Initialize();
    }

    public void FireKilledEvent(int culprit, int victim)
    {
        onKilled?.Invoke(culprit, victim);
    }
}