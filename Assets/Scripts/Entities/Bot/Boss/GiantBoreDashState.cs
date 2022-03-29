using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiantBoreDashState : EntityBaseState
{
    private Transform entity;

    private int dashRepeat = 3;
    private int _dashCount;

    private float aimingTime = 0.75f;
    private float _aimTime;

    private float delay = 0.3f;
    private float _delay;

    private float repeatDashAimTime = 0.5f;
    private float repeatDashDelayTime = 0.5f;

    private float speed;
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
        stateMachine.PathFinder.Stop();

        entity = stateMachine.transform;
        startPos = entity.position;
        originalSpeed = stateMachine.Manager.speed;

        speed = boreStateMachine.dashSpeed;
        _duration = boreStateMachine.dashDuration;
        _delay = delay;
        _aimTime = aimingTime;
    }

    public override void UpdateState()
    {
        if (_aimTime <= 0f)
        {
            if (_delay <= 0f)
            {
                if (_dashCount < dashRepeat)
                {
                    Dash();
                }
            } else
            {
                _delay -= Time.deltaTime;
            }
        }
        else
        {
            if (_dashCount < dashRepeat)
            {
                Vector3 pos = stateMachine.Target.position;
                entity.LookAt(new Vector3(pos.x, entity.position.y, pos.z));

                _aimTime -= Time.deltaTime;
            } else
            {
                SwitchState(factory.BoreIdle());
            }
        }
    }

    private void Dash()
    {

        if (Vector3.Distance(startPos, entity.position) <= boreStateMachine.maxDist || _duration > 0)
        {
            stateMachine.Manager.speed = speed;
            //entity.LookAt(new Vector3(stateMachine.Target.position.x, entity.position.y, stateMachine.Target.position.z));
            stateMachine.PathFinder.movementBehaviour.MoveEntity(speed, entity.forward, stateMachine.PathFinder.rb, entity);
            _duration -= Time.deltaTime;
        } else
        {
            if (_dashCount < dashRepeat)
            {
                stateMachine.PathFinder.rb.velocity = Vector3.zero;
                _aimTime = repeatDashAimTime;
                _delay = repeatDashDelayTime;
                _duration = boreStateMachine.dashDuration;
                _dashCount++;
            }
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
        if (_dashCount > 0)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                SwitchState(factory.BoreIdle());
            }
        }
    }
}
