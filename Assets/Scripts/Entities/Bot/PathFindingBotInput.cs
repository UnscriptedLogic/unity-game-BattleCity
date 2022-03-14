using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BotState
{
    Roam,
    ChasingEnemy,
    ChasingBase,
    StandingGaurd
}

public class PathFindingBotInput : Semaphore
{
    public PathFindingMovement pfMovement;
    private TankManager tankManager;

    public BotState botState;

    public Transform target;
    public LayerMask tankLayer;
    public SphereCollider sphereCollider;

    public Dictionary<BoxCollider, TankManager> enemyTanks = new Dictionary<BoxCollider, TankManager>();

    [Header("Roam")]
    public float roamRange = 10f;
    public float roamInterval = 10f;
    private float _roamInterval;

    [Header("Chase")]
    [Tooltip("The interval in which a new path is requested when chasing a unit")]
    public float recalcChasePath = 2f;
    private float _recalcChasePath;

    public float tankInteractionRange = 3f;
    public float stoppingDistance = 3f;

    [Header("On Gaurd")]
    public float stateCheck = 3f;
    private float _stateCheck;

    private Vector3 pos;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        tankManager = manager as TankManager;

        TankManager[] tankList = TeamManager.instance.GetTanksNotInTeam(TeamManager.instance.GetTeamOfTank(tankManager)).ToArray();
        for (int i = 0; i < tankList.Length; i++)
        {
            enemyTanks.Add(tankList[i].GetComponent<BoxCollider>(), tankList[i]);
        }

        sphereCollider.radius = tankInteractionRange;
        sphereCollider.isTrigger = true;
    }

    private void Start()
    {
        //Vector3 pos = RandomValue.PointAtCircumferenceXZ(transform.position, randomMoveRange);
        //pfMovement.Move(pos);

    }

    private void Update()
    {
        //if (Vector3.Distance(transform.position, target.position) >= stoppingDistance)
        //{
        //    ChaseTarget();
        //} 
        //else
        //{
        //    pfMovement.Stop();
        //    _recalculatePathInterval = recalculatePathInterval;
        //    LookAtTarget();
        //}

        //_recalculatePathInterval -= Time.deltaTime;

        //check if a tank is nearby
        //is tank an enemy => chase
        //is tank an ally => plot
        //else => roam
        switch (botState)
        {
            case BotState.Roam:
                if (Vector3.Distance(pos, transform.position) <= 0.25f)
                {
                    _roamInterval = 0f;
                }

                if (_roamInterval <= 0f)
                {
                    pos = RandomValue.PointAtCircumferenceXZ(transform.position, roamRange);
                    while (Physics.CheckSphere(pos, 0.45f) && Physics.Raycast(pos, Vector3.down, 1f))
                    {
                        pos = RandomValue.PointAtCircumferenceXZ(transform.position, roamRange);
                    }

                    pfMovement.Move(pos);
                    _roamInterval = roamInterval;
                }

                _roamInterval -= Time.deltaTime;
                _recalcChasePath = 0f;
                break;
            case BotState.ChasingEnemy:
                if (_recalcChasePath <= 0f)
                {
                    pfMovement.Move(target.position);
                    _recalcChasePath = recalcChasePath;
                }

                if (Vector3.Distance(target.position, transform.position) <= stoppingDistance)
                {
                    botState = BotState.StandingGaurd;
                }

                _recalcChasePath -= Time.deltaTime;
                break;
            case BotState.ChasingBase:
                break;
            case BotState.StandingGaurd:
                pfMovement.Stop();
                LookAtTarget();

                if (_stateCheck <= 0f)
                {
                    float dist = Vector3.Distance(transform.position, target.position);
                    if (dist <= tankInteractionRange && dist >= stoppingDistance)
                    {
                        botState = BotState.ChasingEnemy;
                    }

                    if (dist >= tankInteractionRange)
                    {
                        botState = BotState.Roam;
                    }

                    _stateCheck = stateCheck;
                }

                _stateCheck -= Time.deltaTime;
                break;
            default:
                break;
        }
    }

    private void LookAtTarget()
    {
        transform.forward = VectorHelper.CorrectToCartesianXZ((target.position - transform.position).normalized);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform)
        {
            if (other.gameObject.layer == Mathf.RoundToInt(Mathf.Log(tankLayer.value, 2)))
            {
                Debug.Log(enemyTanks.Count);

                if (enemyTanks.TryGetValue(other as BoxCollider, out TankManager tankManager))
                {
                    target = tankManager.transform;
                    botState = BotState.ChasingEnemy;
                } 
            }

            //if (other.TryGetComponent(out BulletManager bulletManager))
            //{
            //    if (bulletManager.teamIndex != TeamManager.instance.GetTeamOfTank(tankManager))
            //    {
            //        //Checks if its going to collide with us
            //        if (Physics.Raycast(bulletManager.transform.position, bulletManager.transform.forward, out RaycastHit hit, 50f))
            //        {
            //            if (hit.transform == transform)
            //            {
            //                Debug.Log(Vector3.Dot(transform.forward, bulletManager.transform.forward));
            //            }
            //        }
            //    }
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != transform)
        {
            if (other.gameObject.layer == Mathf.RoundToInt(Mathf.Log(tankLayer.value, 2)))
            {
                Debug.Log(enemyTanks.Count);

                if (enemyTanks.TryGetValue(other as BoxCollider, out TankManager tankManager))
                {
                    target = null;
                    botState = BotState.Roam;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tankInteractionRange);
        
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawRay(transform.position, (target.position - transform.position).normalized * rayDistanceMult);
        //Gizmos.DrawRay(transform.position, VectorHelper.CorrectToCartesianXZ((target.position - transform.position).normalized) * rayDistanceMult);
    }
}