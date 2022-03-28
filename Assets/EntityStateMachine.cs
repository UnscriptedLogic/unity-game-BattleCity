using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStateMachine : Semaphore
{
    private Transform target;
    private Transform homebase;

    private EntityBaseState currentState;
    private EntityStateFactory stateFactory;
    private PathFindingMovement pathFinder;

    private bool initialized;

    public float roamRange = 15f;
    public float attackRange = 10f;
    public float chaseRange = 10f;
    public float tooCloseRange = 10f;

    public Transform Target { get => target; }
    public Transform HomeBase { get => homebase; }
    public PathFindingMovement PathFinder { get => pathFinder; }
    public EntityBaseState CurrentState { get { return currentState; } set { currentState = value; } }

    public bool isTargetInAttackRange { get => Vector3.Distance(transform.position, target.position) <= attackRange; }
    public bool isTargetInChaseRange { get => Vector3.Distance(transform.position, target.position) <= chaseRange; }
    public bool isTargetTooClose { get => Vector3.Distance(transform.position, target.position) <= tooCloseRange; }

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        pathFinder = GetComponent<PathFindingMovement>();
        homebase = BaseBlockManager.instance.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        stateFactory = new EntityStateFactory(this);
        currentState = stateFactory.Roam();
        currentState.EnterState();

        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        currentState.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(collision);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRange);
    }
}
