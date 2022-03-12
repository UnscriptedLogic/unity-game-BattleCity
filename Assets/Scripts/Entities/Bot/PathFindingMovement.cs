﻿using System.Collections;
using UnityEngine;

public class PathFindingMovement : EntityMovement
{
	public Transform target;
	public Rigidbody rb;
	private EntityManager entityManager;
	public float checkRadius;
	Vector3[] path;
	int targetIndex;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
		
		//PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		entityManager = manager as EntityManager;
	}

	public void Move()
	{
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
	}

	public void Stop()
	{
		StopCoroutine("FollowPath");
		path = new Vector3[0];
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	private IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];
		while (true)
		{
            if (Vector3.Distance(transform.position, currentWaypoint) <= checkRadius)
            //if (transform.position == currentWaypoint)
            {
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					yield break;
				}
				currentWaypoint = path[targetIndex];
            } else
            {
				movementBehaviour.Move();
			}

			movementBehaviour.FaceMovement(transform, VectorHelper.CorrectToCartesianXZ((currentWaypoint - transform.position).normalized), entityManager.rotationSpeed, transform);
			//Vector3 lookdir = Vector3.zero;
			//float LRdirection = Vector3.Dot(transform.right, currentWaypoint - transform.position);
			//if (LRdirection >= angleCheckRadius)
			//{
			//    //lookdir = Vector3.left;
			//    transform.forward = transform.right;

			//}
			//else if (LRdirection <= -angleCheckRadius)
			//{
			//    //lookdir = Vector3.right;
			//    transform.forward = -transform.right;
			//    Debug.Log(LRdirection);

			//}

			//Debug.Log(Vector3.Dot(transform.position, currentWaypoint));
			//movementBehaviour.FaceMovement(transform, lookdir, entityManager.rotationSpeed, transform);

			yield return null;

		}
	}

    public override void SetDefaultBehaviour()
    {
		movementBehaviour = new HeadlessLinearMovement(entityManager, rb, transform);
    }

    public void OnDrawGizmos()
	{
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one * 0.5f);
            }
        }

        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}