using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerShoot : EntityShoot
{
    public PlayerInput playerInput;
    private PlayerManager playerManager;
    public Transform bulletAnchor;
    public int airborneBullets = 1;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        playerManager = manager as PlayerManager;
        shootAnchor = bulletAnchor;
    }

    public override void SetDefaultBehaviour()
    {
        shootBehaviour = new IntAirborne(playerManager, shootAnchor, transform, airborneBullets);
        playerInput.RegisterBind(PerformShoot, ActionType.Shoot, EventType.Performed);
    }

    private void PerformShoot(InputAction.CallbackContext obj)
    {
        shootBehaviour.Shoot();
    }
}
