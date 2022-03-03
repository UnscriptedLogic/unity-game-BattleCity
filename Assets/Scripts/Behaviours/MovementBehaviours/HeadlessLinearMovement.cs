using System.Collections;
using UnityEngine;

public class HeadlessLinearMovement : MovementBehaviour
{
    private EntityManager entityManager;
    private Rigidbody rigidbody;
    private Transform transform;

    public HeadlessLinearMovement(EntityManager entityManager, Rigidbody rigidbody, Transform transform)
    {
        this.entityManager = entityManager;
        this.rigidbody = rigidbody;
        this.transform = transform;
    }

    public override void Move()
    {
        MoveEntity(entityManager.speed, transform.forward, rigidbody, transform);
        FaceMovement(transform, transform.forward, entityManager.rotationSpeed, transform);
    }
}