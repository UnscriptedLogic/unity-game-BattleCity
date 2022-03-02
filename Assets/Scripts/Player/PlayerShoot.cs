using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerShoot : EntityShoot
{
    public PlayerInput playerInput;
    private PlayerManager playerManager;
    public Transform shootAnchor;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        playerManager = manager as PlayerManager;
    }

    public override void SetDefaultBehaviour()
    {
        shootBehaviour = new IntAirborne(playerManager, shootAnchor, transform, 2);
        playerInput.RegisterBind(PerformShoot, ActionType.Shoot, EventType.Performed);
    }

    private void PerformShoot(InputAction.CallbackContext obj)
    {
        shootBehaviour.Shoot();
    }
}
