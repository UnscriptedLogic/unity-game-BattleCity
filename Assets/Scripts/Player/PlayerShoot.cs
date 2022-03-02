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

    //public override void Initialize(EntityManager manager)
    //{
    //    playerInput.RegisterBind(PerformShoot, ActionType.Shoot, EventType.Performed);

    //    base.Initialize(manager);
    //}

    //public void PerformShoot(InputAction.CallbackContext context)
    //{
    //    if (listenToInput)
    //    {
    //        if (airborneBullet == null)
    //        {
    //            BulletDetails details = new BulletDetails(
    //                manager.tankSettings.bulletSettings, 
    //                manager.bulletSpeed, 
    //                manager.bulletLifetime, 
    //                transform.GetComponent<TankTeamIndexer>().teamIndex, 
    //                manager.bulletHealth,
    //                manager
    //                );
    //            airborneBullet = CreateBullet(bulletPrefab, shootAnchor, details, out BulletManager bulletScript);
    //        } 
    //    }
    //}

    //private void OnDisable()
    //{
    //    listenToInput = false;
    //}

    //private void OnEnable()
    //{
    //    listenToInput = true;
    //}
}
