using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    public string teamName;
    public Color associatedColor;
    public List<TankManager> entities;
}


public class TeamManager : Semaphore
{
    public static TeamManager instance;
    public Team[] teams;

    public event Action<TankManager, int> onAddedToTeam;
    public event Action<TankManager, int> onRemovedFromTeam;


    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        instance = this;
        TankIndexManager.instance.onTankAdded += IndexManager_onEntityAdded;
        TankIndexManager.instance.onTankRemoved += Instance_onEntiyRemoved;
    }

    private void Instance_onEntiyRemoved(TankManager tankManager)
    {
        TankTeamIndexer indexer = tankManager.GetComponent<TankTeamIndexer>();
        RemoveFromTeam(indexer.teamIndex, tankManager);
        onRemovedFromTeam?.Invoke(tankManager, indexer.teamIndex);
    }

    private void IndexManager_onEntityAdded(TankManager tankManager)
    {
        TankTeamIndexer indexer = tankManager.GetComponent<TankTeamIndexer>();
        AddToTeam(indexer.teamIndex, tankManager);
        onAddedToTeam?.Invoke(tankManager, indexer.teamIndex);
    }

    public void AddToTeam(int index, TankManager entity)
    {
        if (NotValidTeamIndex(index))
        {
            return;
        }

        teams[index].entities.Add(entity);
    }

    public void RemoveFromTeam(int index, TankManager entity)
    {
        if (NotValidTeamIndex(index))
        {
            return;
        }

        teams[index].entities.Remove(entity);
    }

    public void ChangeTeam(TankManager entity, int formerIndex, int newIndex)
    {
        if (NotValidTeamIndex(formerIndex) || NotValidTeamIndex(newIndex))
        {
            return;
        }

        RemoveFromTeam(formerIndex, entity);
        AddToTeam(newIndex, entity);
    }

    public List<TankManager> GetEntitiesInTeam(int index)
    {
        if (NotValidTeamIndex(index))
        {
            return null;
        }

        return teams[index].entities;
    }

    public List<TankManager> GetTanksNotInTeam(int index)
    {

        if (NotValidTeamIndex(index))
        {
            return null;
        }

        List<TankManager> tankManagers = new List<TankManager>();
        for (int currentTeamIndex = 0; currentTeamIndex < teams.Length; currentTeamIndex++)
        {
            if (currentTeamIndex != index)
            {
                for (int i = 0; i < teams[currentTeamIndex].entities.Count; i++)
                {
                    tankManagers.Add(teams[currentTeamIndex].entities[i]);
                }
            }
        }

        return tankManagers;
    }

    public int GetTeamOfTank(TankManager tankManager)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].entities.Contains(tankManager))
            {
                return i;
            }
        }

        return -1;
    }

    public bool isSameTeam(TankManager tankA, TankManager tankB)
    {
        return GetTeamOfTank(tankA) - GetTeamOfTank(tankB) > 0;
    }

    protected bool NotValidTeamIndex(int index)
    {
        if (index >= teams.Length || index < 0)
        {
            Debug.Log("The team with and index of " + index + " does not exist");
            return true;
        }

        return false;
    }
}