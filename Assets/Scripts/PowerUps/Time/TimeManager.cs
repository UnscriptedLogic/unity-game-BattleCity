using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float duration;
    private float _duration;
    private List<TimeAffector> timeAffectors = new List<TimeAffector>();

    private bool activated;
    public int collectorIndex;

    public event Action onTimeResumed;

    public void Update()
    {
        if (activated)
        {
            if (_duration <= 0)
            {
                ResumeTime();
                activated = false;
            }

            Debug.Log("Za Warudo: " + _duration);
            _duration -= Time.deltaTime;
        }
    }

    public void AffectTime()
    {
        //Gets everyone not on the same team, applies the TimeAffector script which actually stops them
        List<TankManager> collectorEnemies = new List<TankManager>(TeamManager.instance.GetTanksNotInTeam(collectorIndex));
        for (int i = 0; i < collectorEnemies.Count; i++)
        {
            timeAffectors.Add(collectorEnemies[i].gameObject.AddComponent<TimeAffector>());
        }

        //For new enemies spawning in
        TeamManager.instance.onAddedToTeam += TeamManager_onAddedToTeam;
        _duration = duration;
        activated = true;
    }

    public void ResumeTime()
    {
        //Destroys all TimeAffectors because TimeAffectors.OnDestroy releases the entites stuck in time
        for (int i = 0; i < timeAffectors.Count; i++)
        {
            Destroy(timeAffectors[i]);
        }

        TeamManager.instance.onAddedToTeam -= TeamManager_onAddedToTeam;
        onTimeResumed?.Invoke();

        //WARNING: This instance that stops the time is running on the GameManager and not on the box anymore
        Destroy(this);
    }

    public void TeamManager_onAddedToTeam(TankManager arg1, int arg2)
    {
        if (arg2 != collectorIndex)
        {
            timeAffectors.Add(arg1.gameObject.AddComponent<TimeAffector>());
        }
    }

    public void ResetTime()
    {
        _duration = duration;
    }
}