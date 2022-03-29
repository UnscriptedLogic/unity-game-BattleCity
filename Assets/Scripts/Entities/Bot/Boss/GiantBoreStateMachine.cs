using System.Collections;
using UnityEngine;

public class GiantBoreStateMachine : EntityStateMachine
{
    [Header("Bore Settings")]
    public float dashSpeed = 1f;
    public float dashDuration = 10f;
    public float maxDist = 5f;

    protected override void StartMachine()
    {
        stateFactory = new EntityStateFactory(this);
        currentState = stateFactory.BoreChaseBase();
        currentState.EnterState();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, maxDist);
    }
}