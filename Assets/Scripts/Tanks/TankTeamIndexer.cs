using System.Collections;
using UnityEngine;

public class TankTeamIndexer : EntitySemaphore
{
    public int teamIndex;
    private TankManager manager;

    public override void Initialize(EntityManager manager)
    {
        manager = GetComponent<TankManager>();
        base.Initialize(manager);
    }

    private void OnDestroy()
    {
        if (manager && TeamManager.instance)
        {
            TeamManager.instance.RemoveFromTeam(teamIndex, manager);
        }
    }
}