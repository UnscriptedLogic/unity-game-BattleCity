using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : EntitySemaphore
{
    public event Action<bool> onEntityMove;

    protected void MoveEntity(float speed, Vector3 direction, Rigidbody rb)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
    }

    protected void FaceMovement(Transform objectToRotate, Vector3 dir, float rotationSpeed)
    {
        if (dir == Vector3.zero)
        {
            return;
        }

        objectToRotate.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
    }

    public void FireEntityMoved(bool value)
    {
        onEntityMove?.Invoke(value);
    }
}
