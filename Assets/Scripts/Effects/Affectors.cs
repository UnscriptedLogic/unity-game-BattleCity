using System.Collections;
using UnityEngine;

public class Affectors : MonoBehaviour
{
    protected float duration;

    public virtual void Activate(float duration)
    {

    }

    public bool TargetAlreadyAffected<T>(out T component)
    {
        component = GetComponent<T>();
        return component != null;
    }
}