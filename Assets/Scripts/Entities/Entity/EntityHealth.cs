using System;
using System.Collections;
using UnityEngine;

public class EntityHealth : Semaphore
{
    private EntityManager entityManager;
    [Tooltip("Leave empty only if this script is on the root of the object.")]
    public GameObject root;

    public event Action<int> onHealthChanged;
    public event Action<int> onHealthDeducted;
    public event Action onKilled;
    
    public bool onDeathDestroys;
    public bool onDeathDisables = false;

    public EntityManager Manager { get => entityManager; }

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        entityManager = manager as EntityManager;
        if (!root)
        {
            root = entityManager.gameObject;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        entityManager.health -= damage;
        if (entityManager.health <= 0)
        {
            entityManager.health = 0;
            KillEntity();
            onKilled?.Invoke();
        }

        onHealthDeducted?.Invoke(damage);
        onHealthChanged?.Invoke(entityManager.health);
    }

    public void KillEntity()
    {
        if (onDeathDestroys)
        {
            Destroy(root);
        } else if (onDeathDisables)
        {
            root.SetActive(false);
        }
    }
}