using System.Collections;
using UnityEngine;

public class GiantBoreChaseBaseState : EntityChaseBaseState
{
    private float chaseCooldown = 1f;

    public GiantBoreChaseBaseState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (chaseCooldown <= 0f)
        {
            if (stateMachine.isTargetInChaseRange)
            {
                SwitchState(factory.BoreDash());
                return;
            }
        } else
        {
            chaseCooldown -= Time.deltaTime;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector3.Distance(Vector3.zero, entity.position) > 50f)
        {
            entity.position = stateMachine.StartPos;
        }
    }
}