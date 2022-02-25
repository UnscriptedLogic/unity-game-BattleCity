using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    StopDown,
    StopLeft,
    StopRight,
    StopUp,
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown
}

public class BotMovement : EntityMovement
{
    public MovementState movementState = MovementState.StopDown;

    public BotManager manager;
    public Rigidbody rb;

    private Vector3 moveDir;
    private Vector3 lookDir;

    private void FixedUpdate()
    {
        MoveEntity(manager.movementSpeed, moveDir, rb);
        FaceMovement(transform, lookDir, manager.rotationSpeed);
    }

    public void SetMovementState(MovementState _movementState)
    {
        movementState = _movementState;

        switch (movementState)
        {
            case MovementState.StopDown:
                moveDir = Vector3.zero;
                lookDir = Vector3.back;
                break;
            case MovementState.StopLeft:
                moveDir = Vector3.zero;
                lookDir = Vector3.left;
                break;
            case MovementState.StopRight:
                moveDir = Vector3.zero;
                lookDir = Vector3.right;
                break;
            case MovementState.StopUp:
                moveDir = Vector3.zero;
                lookDir = Vector3.forward;
                break;
            //Moving
            case MovementState.MoveLeft:
                moveDir = Vector3.left;
                lookDir = moveDir;
                break;
            case MovementState.MoveRight:
                moveDir = Vector3.right;
                lookDir = moveDir;
                break;
            case MovementState.MoveUp:
                moveDir = Vector3.forward;
                lookDir = moveDir;
                break;
            case MovementState.MoveDown:
                moveDir = Vector3.back;
                lookDir = moveDir;
                break;
            default:
                Debug.Log("Something went wrong with settings the movement state", gameObject);
                moveDir = Vector3.zero;
                lookDir = moveDir;
                break;
        }

        FireEntityMoved(moveDir.magnitude > 0);
    }
}
