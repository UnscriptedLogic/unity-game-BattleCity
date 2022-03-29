using System.Collections;
using UnityEngine;

public class EntityIdleState : EntityBaseState
{
    protected Transform entity;
    protected float duration;
    protected float _duration;

    public EntityIdleState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (_duration <= 0f)
        {
            SwitchFromIdleTo();
        }
    }

    public virtual void SwitchFromIdleTo()
    {
        SwitchState(factory.Roam());
    }

    public override void EnterState()
    {
        Debug.Log("Idle State");
        entity = stateMachine.transform;
        duration = stateMachine.idleDuration;
        _duration = duration;
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        _duration -= Time.deltaTime;
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