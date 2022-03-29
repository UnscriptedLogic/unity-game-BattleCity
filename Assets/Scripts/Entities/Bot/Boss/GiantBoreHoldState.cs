using System.Collections;
using UnityEngine;

public class GiantBoreHoldState : EntityHoldState
{
    public GiantBoreHoldState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.isTargetInAttackRange)
        {
            SwitchState(factory.BoreDash());
            return;
        }

        if (!stateMachine.isTargetInChaseRange)
        {
            SwitchState(factory.BoreChaseBase());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        //entity.forward = VectorHelper.CorrectToCartesianXZ((stateMachine.Target.position - entity.position).normalized);
        Vector3 pos = stateMachine.Target.position;
        entity.LookAt(new Vector3(pos.x, entity.position.y, pos.z));
    }
}