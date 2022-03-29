using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntityBaseState
{
    protected EntityStateMachine stateMachine;
    protected EntityStateFactory factory;
    
    protected EntityBaseState(EntityStateMachine ctx, EntityStateFactory factory)
    {
        stateMachine = ctx;
        this.factory = factory;
    }

    //Called once when the state has been first entered
    public abstract void EnterState();
    
    //Acts like a monobehaviour Update function but is only run when the current state is active
    public abstract void UpdateState();

    //Acts like a monobehavior FixedUpdate function but is only run when the current state is active
    public abstract void FixedUpdate();

    //Runs once when the state changed
    public abstract void ExitState();

    public abstract void CheckSwitchCondition();

    //Runs whenever the context monobehaviour detects an OnCollisionEnter
    public abstract void OnCollisionEnter(Collision collision);

    protected void SwitchState(EntityBaseState newState)
    {
        ExitState();

        stateMachine.PathFinder.Stop();
        newState.EnterState();

        stateMachine.CurrentState = newState;
    }
}
