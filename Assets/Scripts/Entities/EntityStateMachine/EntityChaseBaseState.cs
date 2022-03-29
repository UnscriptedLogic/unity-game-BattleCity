using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityChaseBaseState : EntityBaseState
{
    protected Transform entity;
    private float recalculateInterval = 1f;
    private float _interval;

    private Vector3 prevPos;
    private int idleCounter;

    public EntityChaseBaseState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.isTargetTooClose)
        {
            SwitchState(factory.Hold());
            return;
        }
    }

    public override void EnterState()
    {
        Debug.Log("Chase Base State");

        entity = stateMachine.transform;
        prevPos = entity.position;
        _interval = 0f;
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        if (_interval <= 0f)
        {
            stateMachine.PathFinder.Move(stateMachine.HomeBase.position);
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

        if (idleCounter > 100)
        {
            Debug.Log("Caught Lacking");
            idleCounter = 0;
            entity.position = stateMachine.StartPos;
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
