using System;
using System.Collections;
using UnityEngine;

public class EntityHealth : EntitySemaphore
{
    public EntityManager manager;
    [Tooltip("Leave empty only if this script is on the root of the object.")]
    public GameObject root;

    [Obsolete("Event Deprecated", true)]
    public event Action onHealthDepleted;
    
    public event Action<int> onHealthDeducted;
    public event Action<EntityManager> onKilled;
    
    public bool onDeathDestroys;
    public bool onDeathDisables = false;

    public virtual void TakeDamage(int damage)
    {
        if (!manager)
        {
            Debug.Log("EntityHealth is missing a reference to the manager on " + name, gameObject);

            return;
        }

        manager.health -= damage;
        if (manager.health <= 0)
        {
            KillEntity();
        }

        onHealthDeducted?.Invoke(damage);
    }

    public virtual void TakeDamage(int damage, EntityManager source)
    {
        manager.health -= damage;
        if (manager.health <= 0)
        {
            KillEntity();
            onKilled?.Invoke(source);
        }
    }

    //Anonymous Damage
    public void TakeDamage(int amount, int index)
    {
        //Only tanks should check if the damage taken was from another team
        TankTeamIndexer indexer = GetComponent<TankTeamIndexer>();
        if (indexer)
        {
            if (index != indexer.teamIndex)
            {
                TakeDamage(amount, null);
            }
        }
    }

    public virtual void KillEntity()
    {
        onHealthDepleted?.Invoke();
        if (onDeathDestroys)
        {
            if (root)
            {
                Destroy(root);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (onDeathDisables)
        {
            if (root)
            {
                root.SetActive(false);
            } else
            {
                gameObject.SetActive(false);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        
    }
}