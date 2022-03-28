using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityRoamState : EntityBaseState
{
    private Transform entity;

    public EntityRoamState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.isTargetInAttackRange)
        {
            SwitchState(factory.Chase());
        }
    }

    public override void EnterState()
    {
        entity = stateMachine.transform;
        stateMachine.PathFinder.onFailedPath += RandomizeRoamPosition;
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        if (stateMachine.PathFinder.DistToDest <= 2f)
        {
            RandomizeRoamPosition();
        }
    }

    private void RandomizeRoamPosition()
    {
        Vector3 pos = new Vector3();
        bool invalid = true;
        while (invalid)
        {
            pos = RandomValue.PointAtCircumferenceXZ(entity.position, stateMachine.roamRange);
            invalid = !Physics.CheckSphere(pos, 0.45f) && Physics.Raycast(pos, Vector3.down, 1f);
        }

        stateMachine.PathFinder.Move(pos);
    }

    public override void FixedUpdate()
    {

    }

    public override void ExitState()
    {
        stateMachine.PathFinder.Stop();
        stateMachine.PathFinder.onFailedPath -= RandomizeRoamPosition;
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
