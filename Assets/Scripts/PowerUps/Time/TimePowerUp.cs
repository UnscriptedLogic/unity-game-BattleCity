using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePowerUp : PowerUpManager
{
    public float duration;
    private float _duration;
    private List<TimeAffector> timeAffectors = new List<TimeAffector>();

    private bool activated;
    TankTeamIndexer indexer;

    protected override void Start()
    {
        base.Start();

        activated = false;  
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            _lifetime = 999;

            if (_duration > 0)
            {
                AffectTime();

                _duration -= Time.deltaTime;
            }
            else
            {
                activated = false;
            }
        }
    }

    protected override void Activate(Collision collision)
    {
        indexer = collision.transform.GetComponent<TankTeamIndexer>();
        activated = true;
        _duration = duration;

        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetComponent<BoxCollider>());
        Destroy(transform.GetComponent<Rigidbody>());

        base.Activate(collision);
    }

    protected void AffectTime()
    {
        List<TankManager> collectorEnemies = new List<TankManager>();
        collectorEnemies = TeamManager.instance.GetEntitesNotInTeam(indexer.teamIndex);

        for (int i = 0; i < collectorEnemies.Count; i++)
        {
            TimeAffector timeAffector = collectorEnemies[i].GetComponent<TimeAffector>();
            if (!timeAffector)
            {
                timeAffector = collectorEnemies[i].gameObject.AddComponent<TimeAffector>();
            }

            timeAffector.duration = duration;
            timeAffectors.Add(timeAffector);
        }
    }
}
