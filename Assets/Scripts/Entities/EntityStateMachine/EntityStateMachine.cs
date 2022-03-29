using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityStateMachine : Semaphore
{
    protected Transform target;
    protected Transform homebase;

    protected Vector3 startPos;

    protected EntityManager entityManager;
    protected EntityBaseState currentState;
    protected EntityStateFactory stateFactory;
    protected PathFindingMovement pathFinder;

    protected bool initialized;

    public float idleDuration = 5f;
    public float roamRange = 15f;
    public float attackRange = 10f;
    public float chaseRange = 10f;
    public float tooCloseRange = 10f;

    public Transform Target { get => target; }
    public Transform HomeBase { get => homebase; }
    public PathFindingMovement PathFinder { get => pathFinder; }
    public EntityManager Manager { get => entityManager; }
    public EntityBaseState CurrentState { get { return currentState; } set { currentState = value; } }

    public Vector3 StartPos { get => startPos; }
    public float IdleTime { get => idleDuration; }
    public float DistToTarget { get => Vector3.Distance(transform.position, target.position); }
    public bool isTargetInAttackRange { get => Vector3.Distance(transform.position, target.position) <= attackRange; }
    public bool isTargetInChaseRange { get => Vector3.Distance(transform.position, target.position) <= chaseRange; }
    public bool isTargetTooClose { get => Vector3.Distance(transform.position, target.position) <= tooCloseRange; }

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        entityManager = manager as EntityManager;
        pathFinder = GetComponent<PathFindingMovement>();
        homebase = BaseBlockManager.instance.transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;

        StartMachine();

        initialized = true;
    }

    protected virtual void StartMachine()
    {
        stateFactory = new EntityStateFactory(this);
        currentState = stateFactory.Roam();
        currentState.EnterState();
    }

    protected void Update()
    {
        if (!initialized)
        {
            return;
        }

        currentState.UpdateState();
    }

    protected void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(collision);
    }

    protected virtual void OnDrawGizmosSelected()
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
