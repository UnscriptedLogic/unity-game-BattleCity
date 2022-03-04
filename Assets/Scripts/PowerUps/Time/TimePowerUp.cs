using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePowerUp : PowerUpManager
{
    public event Action<TimeManager> onTimeAffected;
    public float duration = 10f;

    protected override void Activate(Collision collision)
    {
        int index = collision.transform.GetComponent<TankTeamIndexer>().teamIndex;

        //Reset the duration of the script that already exists
        TimeManager timeManager = GameManager.instance.GetComponent<TimeManager>();
        if (timeManager && timeManager.collectorIndex == index)
        {
            timeManager.ResetTime();
        } else
        {
            timeManager = GameManager.instance.gameObject.AddComponent<TimeManager>();
            timeManager.duration = duration;
            timeManager.collectorIndex = index;
            timeManager.AffectTime();
        }

        onTimeAffected?.Invoke(timeManager);
        base.Activate(collision);
        Destroy(gameObject);
    }
}
