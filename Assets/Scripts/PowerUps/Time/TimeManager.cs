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

    private float delay = 2f;
    private float _delay;

    private void Start()
    {
        _delay = delay;
    }

    public void Update()
    {
        if (_delay <= 0f)
        {
            if (activated)
            {
                if (_duration <= 0)
                {
                    ResumeTime();
                    activated = false;
                }

                _duration -= Time.deltaTime;
            } else
            {
                AffectTime();
            }
        } else
        {
            _delay -= Time.deltaTime;
        }
    }

    public void AffectTime()
    {
        //Gets everyone not on the same team, applies the TimeAffector script which actually stops them
        List<TankManager> collectorEnemies = new List<TankManager>(TeamManager.instance.GetTanksNotInTeam(collectorIndex));
        for (int i = 0; i < collectorEnemies.Count; i++)
        {
            timeAffectors.Add(collectorEnemies[i].gameObject.AddComponent<TimeAffector>());

            //VFX
            //GameObject particle = Instantiate(AssetManager.instance.timeStopAura, collectorEnemies[i].transform);
            //if (Physics.Raycast(collectorEnemies[i].transform.position, Vector3.down, out RaycastHit hit, 10f))
            //{
            //    particle.transform.position = hit.point;
            //}

            //BoxCollider boxCollider = collectorEnemies[i].transform.GetComponent<BoxCollider>();
            //if (boxCollider)
            //{
            //    float scale = Mathf.Max(boxCollider.size.x, boxCollider.size.z) / 1.5f;
            //    particle.transform.localScale = new Vector3(scale, 1f, scale);
            //}

            //Destroy(particle, 10.5f);
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