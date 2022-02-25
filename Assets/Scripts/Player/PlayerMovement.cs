using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EntityMovement
{
    public PlayerInput playerInput;
    public PlayerManager manager;
    private Rigidbody rb;
    private Vector3 moveVector;

    public override void Initialize(EntityManager manager)
    {
        rb = GetComponent<Rigidbody>();

        playerInput.RegisterBind(GetMoveDir, ActionType.Move, EventType.Performed);
        playerInput.RegisterBind(GetMoveDir, ActionType.Move, EventType.Cancelled);

        base.Initialize(manager);
    }

    private void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;
        if (Mathf.Abs(moveVector.x) >= 0.1f)
        {
            dir = Vector3.right * moveVector.x;
            MoveEntity(manager.movementSpeed, dir, rb);
            FaceMovement(transform, dir, manager.rotationSpeed);
        } else if (Mathf.Abs(moveVector.z) >= 0.1f)
        {
            dir = Vector3.forward * moveVector.z;
            MoveEntity(manager.movementSpeed, dir, rb);
            FaceMovement(transform, dir, manager.rotationSpeed);
        }
    }

    private void GetMoveDir(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        moveVector = new Vector3(value.x, 0f, value.y);

        FireEntityMoved(moveVector.magnitude > 0f);
    }
}
