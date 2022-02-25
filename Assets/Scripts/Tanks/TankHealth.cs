using System;
using System.Collections;
using UnityEngine;

public class TankHealth : EntityHealth
{
    private TankManager tankManager;
    public event Action<EntityManager> onKill;

    public override void Initialize(EntityManager manager)
    {
        tankManager = (TankManager)manager;
        onKilled += AnnounceDeathBy;

        base.Initialize(manager);
    }

    public void AnnounceDeathBy(EntityManager source)
    {
        //Signal scripts that a kill has occured
        TankManager sourceManager = source as TankManager;
        sourceManager.healthScript.Killed(manager);
        
        //Signals scripts that a specific tank has killed another specific tank
        DeathManager.instance.FireKilledEvent(sourceManager.tankIndex, tankManager.tankIndex);
    }

    public void Killed(EntityManager victim)
    {
        onKill?.Invoke(victim);
    }

    public override void TakeDamage(int amount, EntityManager culprit)
    {
        //Only tanks should check if the damage taken was from another team
        TankManager culpritManager = culprit as TankManager;
        if (culpritManager)
        {
            if (culpritManager.GetComponent<TankTeamIndexer>().teamIndex != tankManager.GetComponent<TankTeamIndexer>().teamIndex)
            {
                base.TakeDamage(amount, culprit);
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {

    }

    private void OnDestroy()
    {
        IndexManager.instance.RemoveEntity(tankManager.tankIndex);
    }
}