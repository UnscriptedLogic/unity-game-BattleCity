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

    public void MoveEntity(float speed, Vector3 direction, Rigidbody rb, Transform transform)
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

    public void FaceMovement(Transform objectToRotate, Vector3 dir, float rotationSpeed, Transform transform)
    {
        if (dir == Vector3.zero)
        {
            return;
        }
        //dir = new Vector3(dir.x, transform.position.y, dir.z);
        //dir = new Vector3(transform.position.x, dir.y, transform.position.z);
        objectToRotate.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
    }

    public Vector3 Snap(Vector3 angle, float degrees = 90f)
    {
        angle.y = Mathf.Round(angle.y * degrees) / degrees;
        return angle;
    }

    public void FireEntityMoved(bool value)
    {
        onEntityMove?.Invoke(value);
    }
}