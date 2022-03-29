using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHoldState : EntityBaseState
{
    protected Transform entity;

    public EntityHoldState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.DistToTarget > stateMachine.tooCloseRange)
        {
            SwitchState(factory.Roam());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Hold State");

        entity = stateMachine.transform;
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();

        entity.forward = VectorHelper.CorrectToCartesianXZ((stateMachine.Target.position - entity.position).normalized);
    }

    public override void FixedUpdate()
    {

    }

    public override void ExitState()
    {

    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
