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

    public BotState currState;

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

        //A catch just in case there is no base in the scene
        homeBase = BaseBlockManager.instance != null ? BaseBlockManager.instance.transform : transform;

        onDefaultBehaviour?.Invoke();
    }

    private void Update()
    {
        UpdateState();
    }

    public void EnterState()
    {
        switch (currState)
        {
            case BotState.Roam:
                _roamInterval = 0f;
                break;
            case BotState.AttackingEnemy:
                int random = RandomValue.FromIntZeroTo(100);
                if (random <= coordMatchChance)
                {
                    SwitchState(BotState.CoordMatching);
                }
                else
                {
                    SwitchState(BotState.ChasingEnemy);
                }
                break;
            case BotState.CoordMatching:
                _repositionInterval = 0f;
                break;
            case BotState.ChasingEnemy:
                _recalcChasePath = 0f;
                break;
            case BotState.ChasingBase:
                pfMovement.Move(homeBase.position);
                target = homeBase;
                break;
            case BotState.StandingGaurd:
                pfMovement.Stop();
                break;
            default:
                break;
        }
    }

    public void UpdateState()
    {
        switch (currState)
        {
            case BotState.Roam:
                //Close to end point
                if (Vector3.Distance(pos, transform.position) <= 0.25f)
                {
                    _roamInterval = 0f;
                }

                //Roam logic
                if (_roamInterval <= 0f)
                {
                    pos = RandomValue.PointAtCircumferenceXZ(transform.position, roamRange);
                    while (Physics.CheckSphere(pos, 0.45f) && Physics.Raycast(pos, Vector3.down, 1f))
                    {
                        pos = RandomValue.PointAtCircumferenceXZ(transform.position, roamRange);
                    }

                    pfMovement.Move(pos);
                    _roamInterval = roamInterval;
                } else
                {
                    _roamInterval -= Time.deltaTime;
                }
                break;

            case BotState.AttackingEnemy:
                //Merely decides what type of attacking mode to switch to at EnterState();
                break;

            case BotState.CoordMatching:
                //Reposition ever now and then
                if (_repositionInterval <= 0)
                {
                    currOffset = 0f;
                    SingleCoordMatch();
                    _repositionInterval = repositionInterval;
                } else
                {
                    _repositionInterval -= Time.deltaTime;
                }

                //If the point is close enough
                if (Vector3.Distance(coordMatchPos, transform.position) <= 0.5f)
                {
                    pfMovement.Stop();
                    LookAtTarget();

                    float angle = Vector3.Angle(transform.forward, target.position - transform.position);
                    if (angle <= 5f)
                    {
                        if (notClearLOS())
                        {
                            //Chase enemy instead if the target is trying to hide
                            SwitchState(BotState.ChasingEnemy);
                        }
                    }
                }
                break;

            case BotState.ChasingEnemy:
                if (_recalcChasePath <= 0f)
                {
                    //Chases the point infront of the target instead to prevent trailing
                    Vector3 position = target.position + (target.transform.forward * 2f);
                    if (!ValidPosition(position))
                    {
                        //Chases the target itself if no valid position infront of the target is valid
                        position = target.position;
                    }

                    pfMovement.Move(position);
                    _recalcChasePath = recalcChasePath;
                } else
                {
                    _recalcChasePath -= Time.deltaTime;
                }

                //Stand gaurd if the target is close
                if (Vector3.Distance(target.position, transform.position) <= stoppingDistance)
                {
                    SwitchState(BotState.StandingGaurd);
                }
                break;

            case BotState.ChasingBase:
                
                if (Vector3.Distance(transform.position, homeBase.position) <= stoppingDistance)
                {
                    pfMovement.Stop();
                    LookAtTarget();
                }
                break;
            case BotState.StandingGaurd:
                //Constantly Look at target
                LookAtTarget();

                //State check to see if the target is still in range
                if (_stateCheck <= 0f)
                {
                    float dist = Vector3.Distance(transform.position, target.position);
                    if (dist <= tankInteractionRange && dist >= stoppingDistance)
                    {
                        SwitchState(BotState.AttackingEnemy);
                    }

                    if (dist >= tankInteractionRange)
                    {
                        SwitchState(BotState.Roam);
                    }

                    _stateCheck = stateCheck;
                } else
                {
                    _stateCheck -= Time.deltaTime;
                }
                break;
            default:
                break;
        }
    }

    public void ExitState()
    {
        switch (currState)
        {
            case BotState.Roam:
                break;
            case BotState.AttackingEnemy:
                break;
            case BotState.CoordMatching:
                break;
            case BotState.ChasingEnemy:
                break;
            case BotState.ChasingBase:
                break;
            case BotState.StandingGaurd:
                break;
            default:
                break;
        }
    }

    public void SwitchState(BotState newState)
    {
        ExitState();
        currState = newState;
        EnterState();
    }

    public void SetChase()
    {
        SwitchState(BotState.AttackingEnemy);
    }

    public void SetChaseBase()
    {
        SwitchState(BotState.ChasingBase);
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
        Debug.Log(target);

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
        if (other.gameObject.layer == Mathf.RoundToInt(Mathf.Log(tankLayer.value, 2)))
        {
            if (TeamManager.instance.GetTeamOfTank(other.GetComponent<TankManager>()) != TeamManager.instance.GetTeamOfTank(tankManager))
            {
                target = other.transform;
                onEnemyInRange?.Invoke(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
            currState = BotState.Roam;
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