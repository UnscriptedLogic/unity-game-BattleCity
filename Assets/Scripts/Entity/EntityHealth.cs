using System;
using System.Collections;
using UnityEngine;

public class EntityHealth : Semaphore
{
    private EntityManager entityManager;
    [Tooltip("Leave empty only if this script is on the root of the object.")]
    public GameObject root;

    [Obsolete("Event Deprecated", true)]
    public event Action onHealthDepleted;
    
    public event Action<int> onHealthDeducted;
    public event Action<EntityManager> onKilled;
    
    public bool onDeathDestroys;
    public bool onDeathDisables = false;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        entityManager = manager as EntityManager;
        if (!root)
        {
            root = entityManager.gameObject;
        }
    }

    public void TakeDamage(int damage)
    {
        entityManager.health -= damage;
        if (entityManager.health <= 0)
        {
            KillEntity();
        }
    }

    public void KillEntity()
    {
        if (onDeathDestroys)
        {
            Destroy(root);
        } else
        {
            root.SetActive(false);
        }
    }
}