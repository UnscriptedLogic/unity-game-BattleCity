using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : Semaphore
{
    protected MovementBehaviour movementBehaviour;

    public virtual void SetDefaultBehaviour()
    {
        //Overridden in their specific classes for what default movement it is meant to do
    }

    public void SetMovementBehaviour(MovementBehaviour newBehaviour)
    {
        movementBehaviour = newBehaviour;
    }
}
