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


public class TeamManager : Manager
{
    public static TeamManager instance;
    public Team[] teams;

    public override void Initialize()
    {
        instance = this;

        IndexManager.instance.onEntityAdded += IndexManager_onEntityAdded;
        IndexManager.instance.onEntiyRemoved += Instance_onEntiyRemoved;

        base.Initialize();
    }

    private void Instance_onEntiyRemoved(Transform obj)
    {
        TankManager tankManager = obj.GetComponent<TankManager>();
        TankTeamIndexer indexer = obj.GetComponent<TankTeamIndexer>();
        RemoveFromTeam(indexer.teamIndex, tankManager);
    }

    private void IndexManager_onEntityAdded(Transform obj)
    {
        TankManager manager = obj.GetComponent<TankManager>();
        TankTeamIndexer indexer = obj.GetComponent<TankTeamIndexer>();

        AddToTeam(indexer.teamIndex, manager);
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

    public List<TankManager> GetEntitesNotInTeam(int index)
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