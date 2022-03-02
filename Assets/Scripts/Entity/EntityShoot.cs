using System;
using System.Collections;
using UnityEngine;

public class EntityShoot : Sephamore
{
    protected ShootBehaviour shootBehaviour;

    public virtual void SetDefaultBehaviour()
    {
        //Default shooting behaviour
    }

    public void SetShootBehaviour(ShootBehaviour shootBehaviour)
    {
        this.shootBehaviour = shootBehaviour;
    }
}