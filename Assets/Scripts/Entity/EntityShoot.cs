using System;
using System.Collections;
using UnityEngine;

public class EntityShoot : Semaphore
{
    protected ShootBehaviour shootBehaviour;
    protected ShootBehaviour prevShootBehaviour;

    public virtual void SetDefaultBehaviour()
    {
        //Default shooting behaviour
    }

    public void SetShootBehaviour(ShootBehaviour shootBehaviour)
    {
        this.shootBehaviour = shootBehaviour;
    }

    protected virtual void OnEnable()
    {
        SetShootBehaviour(prevShootBehaviour);
    }

    protected virtual void OnDisable()
    {
        prevShootBehaviour = shootBehaviour;
        SetShootBehaviour(new NoShootBehaviour());
    }
}