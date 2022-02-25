using System.Collections;
using UnityEngine;

public class EntitySemaphore : MonoBehaviour
{
    public virtual void Initialize(EntityManager entityManager)
    {
        entityManager.ReleaseGate(this);
    }
}