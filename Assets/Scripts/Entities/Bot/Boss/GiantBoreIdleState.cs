using System.Collections;
using UnityEngine;

public class GiantBoreIdleState : EntityIdleState
{
    public GiantBoreIdleState(EntityStateMachine ctx, EntityStateFactory factory) : base(ctx, factory)
    {
        
    }

    public override void CheckSwitchCondition()
    {
        //Promp a premature dash if the player is really close to the boss
        if (stateMachine.isTargetTooClose && stateMachine.Target.GetComponent<EntityManager>().health > 0)
        {
            Vector3 pos = stateMachine.Target.position;
            entity.LookAt(new Vector3(pos.x, entity.position.y, pos.z));
            SwitchState(factory.BoreDash());
            return;
        }

        if (_duration <= 0f)
        {
            //if (stateMachine.isTargetInAttackRange)
            //{
            //    SwitchState(factory.BoreDash());
            //} else
            //{
            //    SwitchState(factory.BoreChaseBase());
            //}
            SwitchState(factory.BoreChaseBase());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchCondition();
        _duration -= Time.deltaTime;
    }
}