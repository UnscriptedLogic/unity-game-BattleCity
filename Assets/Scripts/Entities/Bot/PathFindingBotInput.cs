using System.Collections;
using UnityEngine;

public enum BotState
{
    ChasePlayer
}

public class PathFindingBotInput : MonoBehaviour
{
    public PathFindingMovement pfMovement;
    public float randomMoveRange = 5f;

    public Transform target;
    public float stoppingDistance = 3f;

    public float recalculatePathInterval = 2f;
    private float _recalculatePathInterval;

    [Header("Debug")]
    public float rayDistanceMult = 2f;

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) >= stoppingDistance)
        {
            if (pfMovement.target != target)
            {
                pfMovement.target = target;
            }

            if (_recalculatePathInterval <= 0f)
            {
                pfMovement.Move();
                _recalculatePathInterval = recalculatePathInterval;
            }
        } else
        {
            pfMovement.Stop();
            _recalculatePathInterval = recalculatePathInterval;
            LookAtTarget();
        }

        _recalculatePathInterval -= Time.deltaTime;
    }

    private void LookAtTarget()
    {
        transform.forward = VectorHelper.CorrectToCartesianXZ((target.position - transform.position).normalized);
    }

    private void ChaseTarget()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, randomMoveRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        Gizmos.DrawRay(transform.position, (target.position - transform.position).normalized * rayDistanceMult);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, VectorHelper.CorrectToCartesianXZ((target.position - transform.position).normalized) * rayDistanceMult);
    }
}