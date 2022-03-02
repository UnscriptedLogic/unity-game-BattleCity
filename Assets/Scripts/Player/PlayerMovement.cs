using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EntityMovement
{
    public Rigidbody rb;
    private PlayerManager playerManager;

    protected override void SephamoreStart(Manager manager)
    {
        playerManager = manager as PlayerManager;
        base.SephamoreStart(manager);
    }

    public override void SetDefaultBehaviour()
    {
        movementBehaviour = new UserInputMovement(playerManager.playerInput, playerManager, rb, transform);
    }

    public void SetBehaviour(MovementBehaviour movementBehaviour)
    {
        this.movementBehaviour = movementBehaviour;
    }

    private void FixedUpdate()
    {
        movementBehaviour.Move();
    }
}
