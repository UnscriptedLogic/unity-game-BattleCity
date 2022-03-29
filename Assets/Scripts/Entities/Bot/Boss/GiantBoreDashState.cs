using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiantBoreDashState : EntityBaseState
{
    private Transform entity;

    private float delay = 0.75f;
    private float _delay;

    private float speed = 10;
    private float _duration;

    private float originalSpeed;
    private Vector3 startPos;
    private GiantBoreStateMachine boreStateMachine;

    public GiantBoreDashState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {

    }

    public override void EnterState()
    {
        Debug.Log("Dash State");
        boreStateMachine = stateMachine as GiantBoreStateMachine;

        entity = stateMachine.transform;
        speed = boreStateMachine.dashSpeed;
        _duration = boreStateMachine.dashDuration;
        startPos = entity.position;

        originalSpeed = stateMachine.Manager.speed;
        _delay = delay;
    }

    public override void UpdateState()
    {
        if (_delay > 0)
        {
            _delay -= Time.deltaTime;
            return;
        }

        if (Vector3.Distance(startPos, entity.position) <= boreStateMachine.maxDist && stateMachine.Target.gameObject.activeInHierarchy)
        {
            if (_duration > 0)
            {
                stateMachine.Manager.speed = speed;
                //entity.LookAt(new Vector3(stateMachine.Target.position.x, entity.position.y, stateMachine.Target.position.z));
                stateMachine.PathFinder.movementBehaviour.MoveEntity(speed, entity.forward, stateMachine.PathFinder.rb, entity);
                _duration -= Time.deltaTime;
            } else
            {
                SwitchState(factory.BoreIdle());
                return;
            }
        } else
        {
            SwitchState(factory.BoreIdle());
            return;
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void ExitState()
    {
        stateMachine.PathFinder.rb.velocity = Vector3.zero;
        stateMachine.Manager.speed = originalSpeed;
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
