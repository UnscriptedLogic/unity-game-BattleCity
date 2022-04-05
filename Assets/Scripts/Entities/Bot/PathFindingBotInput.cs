using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum BotState
{
    Roam,
    AttackingEnemy,
    CoordMatching,
    ChasingEnemy,
    ChasingBase,
    StandingGaurd
}

public class PathFindingBotInput : Semaphore
{
    public PathFindingMovement pfMovement;
    private TankManager tankManager;

    public BotState botState;

    public LayerMask tankLayer;
    public SphereCollider sphereCollider;

    public Dictionary<BoxCollider, TankManager> enemyTanks = new Dictionary<BoxCollider, TankManager>();

    private Transform target;
    private Transform homeBase;

    public UnityEvent onDefaultBehaviour;
    public UnityEvent<Transform> onEnemyInRange;

    [Header("Roam")]
    public float roamRange = 10f;
    public float roamInterval = 10f;
    private float _roamInterval;

    [Header("Attacking")]
    [Range(0, 100)]
    public int coordMatchChance = 50;

    [Header("Chase")]
    [Tooltip("The interval in which a new path is requested when chasing a unit.")]
    public float recalcChasePath = 2f;
    private float _recalcChasePath;

    public float tankInteractionRange = 3f;
    public float stoppingDistance = 3f;

    [Header("On Gaurd")]
    public float stateCheck = 3f;
    private float _stateCheck;

    [Header("Coord Matching")]
    public float coordFailOffset;
    public float repositionInterval = 3f;
    private float _repositionInterval;
    private Vector3 coordMatchPos;

    [Header("Line of Sight")]
    public LayerMask blockLayer;
    private float currOffset;
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

        homeBase = BaseBlockManager.instance.transform;
        onDefaultBehaviour?.Invoke();
    }

    private void Update()
    {
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
            case BotState.AttackingEnemy:
                int random = RandomValue.FromIntZeroTo(100);
                if (random <= coordMatchChance)
                {
                    botState = BotState.CoordMatching;
                } else
                {
                    botState = BotState.ChasingEnemy;
                }
                break;
            case BotState.ChasingEnemy:
                if (_recalcChasePath <= 0f)
                {
                    Vector3 position = target.position + (target.transform.forward * 2f);
                    if (!ValidPosition(position))
                    {
                        position = target.position;
                    }

                    pfMovement.Move(position);
                    _recalcChasePath = recalcChasePath;
                }

                if (Vector3.Distance(target.position, transform.position) <= stoppingDistance)
                {
                    botState = BotState.StandingGaurd;
                }

                _recalcChasePath -= Time.deltaTime;
                break;
            case BotState.CoordMatching:
                if (_repositionInterval <= 0)
                {
                    currOffset = 0f;
                    SingleCoordMatch();
                    _repositionInterval = repositionInterval;
                }

                if (Vector3.Distance(coordMatchPos, transform.position) <= 0.5f)
                {
                    pfMovement.Stop();
                    LookAtTarget();

                    float angle = Vector3.Angle(transform.forward, target.position - transform.position);
                    if (angle <= 5f)
                    {
                        Debug.Log(notClearLOS());
                        if (notClearLOS())
                        {
                            botState = BotState.ChasingEnemy;
                        }
                    }
                }

                _repositionInterval -= Time.deltaTime;
                break;
            case BotState.ChasingBase:
                if (target != homeBase)
                {
                    target = homeBase;
                    pfMovement.Move(homeBase.position);
                    if (Vector3.Distance(transform.position, homeBase.position) <= stoppingDistance)
                    {
                        pfMovement.Stop();
                        LookAtTarget();
                    }
                }
                break;
            case BotState.StandingGaurd:
                pfMovement.Stop();
                LookAtTarget();

                if (_stateCheck <= 0f)
                {
                    float dist = Vector3.Distance(transform.position, target.position);
                    if (dist <= tankInteractionRange && dist >= stoppingDistance)
                    {
                        botState = BotState.AttackingEnemy;
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

        if (target == null)
        {
            target = null;
            botState = BotState.Roam;
        }
    }

    public void SetChase()
    {
        botState = BotState.AttackingEnemy;
    }

    public void SetChaseBase()
    {
        botState = BotState.ChasingBase;
    }

    private bool ValidPosition(Vector3 position)
    {
        //If there is no obstacle and there is ground
        return !Physics.CheckSphere(coordMatchPos, 0.45f) && Physics.Raycast(coordMatchPos, Vector3.down, 1f);
    }

    private void LookAtTarget()
    {
        transform.forward = VectorHelper.CorrectToCartesianXZ((target.position - transform.position).normalized);
    }

    private void SingleCoordMatch()
    {
        coordMatchPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
        while (ValidPosition(coordMatchPos))
        {
            coordMatchPos = new Vector3(target.position.x, transform.position.y, transform.position.z - currOffset);
            currOffset += coordFailOffset;
        }
        pfMovement.Move(coordMatchPos);
    }

    private bool notClearLOS()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50f, blockLayer))
        {
            Debug.Log(hit.transform.name);

            BlockManager blockManager = hit.transform.GetComponent<BlockManager>();
            if (blockManager)
            {
                if (blockManager.myWallType == BlockName.Fortified)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform)
        {
            if (other.gameObject.layer == Mathf.RoundToInt(Mathf.Log(tankLayer.value, 2)))
            {
                //if (other as BoxCollider != null)
                //{
                //    if (enemyTanks.TryGetValue(other as BoxCollider, out TankManager tankManager))
                //    {
                //        target = tankManager.transform;
                //        botState = BotState.AttackingEnemy;
                //    }
                //}

                Debug.Log(other.transform.name);

                Debug.Log(TeamManager.instance.GetTeamOfTank(other.GetComponent<TankManager>()));
                if (TeamManager.instance.GetTeamOfTank(other.GetComponent<TankManager>()) != TeamManager.instance.GetTeamOfTank(tankManager))
                {
                    if (target != null)
                    {
                        if (Vector3.Distance(transform.position, other.transform.position) <= Vector3.Distance(transform.position, target.position))
                        {
                            target = other.transform;
                            onEnemyInRange?.Invoke(other.transform);
                            return;
                        } 
                    }

                    target = other.transform;
                    onEnemyInRange?.Invoke(other.transform);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.transform != transform)
        //{
        //    if (other.gameObject.layer == Mathf.RoundToInt(Mathf.Log(tankLayer.value, 2)))
        //    {
        //        if (other as BoxCollider != null)
        //        {
        //            if (enemyTanks.TryGetValue(other as BoxCollider, out TankManager tankManager))
        //            {
        //                target = null;
        //                botState = BotState.Roam;
        //            }
        //        }
        //    }
        //}

        if (other.transform == target)
        {
            target = null;
            botState = BotState.Roam;
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

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(coordMatchPos, 0.5f);
    }
}