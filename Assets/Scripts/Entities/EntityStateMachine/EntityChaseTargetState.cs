using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityChaseTargetState : EntityBaseState
{
    private Transform entity;
    private float recalculateInterval = 1f;
    private float _interval;

    private Vector3 prevPos;
    private int idleCounter;

    public EntityChaseTargetState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.isTargetTooClose)
        {
            SwitchState(factory.Hold());
            return;
        }

        if (stateMachine.DistToTarget > stateMachine.chaseRange)
        {
            SwitchState(factory.Roam());
            return;
        }
    }

    public override void EnterState()
    {
        Debug.Log("Chase State");

        entity = stateMachine.transform;
        prevPos = entity.position;
        _interval = 0f;
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        if (_interval <= 0f)
        {
            stateMachine.PathFinder.Move(stateMachine.Target.position);
            _interval = recalculateInterval;
        } else
        {
            _interval -= Time.deltaTime;
        }
    }

    public override void FixedUpdate()
    {
        if (prevPos == entity.position)
        {
            idleCounter++;
        }
        else
        {
            idleCounter = 0;
        }

        if (idleCounter > 10)
        {
            Debug.Log("Caught Lacking");
            idleCounter = 0;
            SwitchState(factory.Roam());
        }

        prevPos = entity.position;
    }

    public override void ExitState()
    {
        stateMachine.PathFinder.Stop();
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
