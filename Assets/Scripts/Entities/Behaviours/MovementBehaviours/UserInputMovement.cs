using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputMovement : MovementBehaviour
{
    private PlayerInput input;
    private PlayerManager manager;
    private Rigidbody rigidbody;
    private Transform transform;

    private Vector3 moveVector;

    public UserInputMovement(PlayerInput input, PlayerManager manager, Rigidbody rigidbody, Transform transform)
    {
        this.input = input;
        this.manager = manager;
        this.rigidbody = rigidbody;
        this.transform = transform;

        input.RegisterBind(GetMoveDir, ActionType.Move, EventType.Performed);
        input.RegisterBind(GetMoveDir, ActionType.Move, EventType.Cancelled);
    }

    public override void Move()
    {
        Vector3 dir = Vector3.zero;
        if (Mathf.Abs(moveVector.x) >= 0.1f)
        {
            dir = Vector3.right * moveVector.x;
            MoveEntity(manager.speed, dir, rigidbody, transform);
            FaceMovement(transform, dir, manager.rotationSpeed, transform);
        }
        else if (Mathf.Abs(moveVector.z) >= 0.1f)
        {
            dir = Vector3.forward * moveVector.z;
            MoveEntity(manager.speed, dir, rigidbody, transform);
            FaceMovement(transform, dir, manager.rotationSpeed, transform);
        }
    }

    private void GetMoveDir(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        moveVector = new Vector3(value.x, 0f, value.y);

        FireEntityMoved(moveVector.magnitude > 0f);
    }
}