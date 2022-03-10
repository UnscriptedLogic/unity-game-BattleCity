using System.Collections;
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

public class BotRandomDirectionMovement : MovementBehaviour
{
    private BotManager botManager;
    private RandomBotInput botInput;
    private Rigidbody rigidbody;
    private Transform transform;

    private Vector3 moveDir;
    private Vector3 lookDir;

    public BotRandomDirectionMovement(BotManager botManager, RandomBotInput botInput, Rigidbody rigidbody, Transform transform)
    {
        this.botManager = botManager;
        this.botInput = botInput;
        this.rigidbody = rigidbody;
        this.transform = transform;

        botInput.moveStateUpdated += SetMovementState;
    }

    public override void Move()
    {
        MoveEntity(botManager.speed, moveDir, rigidbody, transform);
        FaceMovement(transform, lookDir, botManager.rotationSpeed, transform);
    }

    public void SetMovementState(MovementState _movementState)
    {
        switch (_movementState)
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
                Debug.Log("Something went wrong with settings the movement state");
                moveDir = Vector3.zero;
                lookDir = moveDir;
                break;
        }
    }
}