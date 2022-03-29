using System.Collections;
using UnityEngine;

public class BoreIdleState : EntityIdleState
{
    public BoreIdleState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
        
    }

    public override void CheckSwitchCondition()
    {
        if (stateMachine.isTargetTooClose && stateMachine.Target.GetComponent<EntityManager>().health > 0)
        {
            Vector3 pos = stateMachine.Target.position;
            entity.LookAt(new Vector3(pos.x, entity.position.y, pos.z));
            SwitchState(factory.BoreDash());
            return;
        }

        if (_duration <= 0f)
        {
            SwitchState(factory.BoreChaseBase());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        _duration -= Time.deltaTime;
    }
}