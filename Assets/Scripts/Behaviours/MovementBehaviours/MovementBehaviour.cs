using System;
using System.Collections;
using UnityEngine;

public class MovementBehaviour
{
    public event Action<bool> onEntityMove;
    private bool fired;

    public virtual void Move()
    {
        //Movement Logic
    }

    protected void MoveEntity(float speed, Vector3 direction, Rigidbody rb, Transform transform)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));

        if (direction.magnitude > 0f)
        {
            if (!fired)
            {
                FireEntityMoved(true);
                fired = true;
            }
        } else
        {
            if (fired)
            {
                FireEntityMoved(false);
                fired = false;
            }
        }
    }

    protected void FaceMovement(Transform objectToRotate, Vector3 dir, float rotationSpeed, Transform transform)
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