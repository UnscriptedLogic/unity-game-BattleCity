using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityRoamState : EntityBaseState
{
    private Transform entity;
    private Vector3 prevPos;
    private int idleCounter;
    private int stuckCounter;

    public EntityRoamState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.isTargetInAttackRange)
        {
            SwitchState(factory.ChaseTarget());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Roam State");

        entity = stateMachine.transform;
        stateMachine.PathFinder.onFailedPath += RandomizeRoamPosition;
        prevPos = entity.position;
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
            invalid = !Physics.CheckSphere(pos, 0.45f) && Physics.Raycast(pos, pos + Vector3.down, 1f);
        }

        stateMachine.PathFinder.Move(pos);
    }

    public override void FixedUpdate()
    {
        if (prevPos == entity.position)
        {
            idleCounter++;
        } else
        {
            idleCounter = 0;
            stuckCounter = 0;
        }

        if (idleCounter > 10)
        {
            Debug.Log("Caught Lacking");
            RandomizeRoamPosition();
            
            idleCounter = 0;
            stuckCounter++;
        }

        if (stuckCounter > 25)
        {
            Debug.Log("Actually Stuck");

            entity.position = stateMachine.StartPos;
            stuckCounter = 0;
            idleCounter = 0;
        }

        prevPos = entity.position;
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
