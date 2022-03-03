using System.Collections;
using UnityEngine;

public class TankTeamIndexer : Semaphore
{
    public int teamIndex;
    private TankManager tankManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        tankManager = manager as TankManager;
    }

    //public override void Initialize(EntityManager manager)
    //{
    //    manager = GetComponent<TankManager>();
    //    base.Initialize(manager);
    //}

    //private void OnDestroy()
    //{
    //    if (manager && TeamManager.instance)
    //    {
    //        TeamManager.instance.RemoveFromTeam(teamIndex, manager);
    //    }
    //}
}