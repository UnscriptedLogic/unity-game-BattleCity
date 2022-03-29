using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntitySnipeState : EntityBaseState
{
    public EntitySnipeState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {

    }

    public override void EnterState()
    {
        Debug.Log("Snipe State");

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
