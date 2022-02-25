using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerShoot : EntityShoot
{
    public PlayerInput playerInput;
    public PlayerManager manager;

    public GameObject bulletPrefab;
    public Transform shootAnchor;
    private GameObject airborneBullet;

    private bool listenToInput = true;

    public override void Initialize(EntityManager manager)
    {
        playerInput.RegisterBind(PerformShoot, ActionType.Shoot, EventType.Performed);

        base.Initialize(manager);
    }

    public void PerformShoot(InputAction.CallbackContext context)
    {
        if (listenToInput)
        {
            if (airborneBullet == null)
            {
                BulletDetails details = new BulletDetails(
                    manager.tankSettings.bulletSettings, 
                    manager.bulletSpeed, 
                    manager.bulletLifetime, 
                    transform.GetComponent<TankTeamIndexer>().teamIndex, 
                    manager.bulletHealth,
                    manager
                    );
                airborneBullet = CreateBullet(bulletPrefab, shootAnchor, details, out BulletManager bulletScript);
            } 
        }
    }

    private void OnDisable()
    {
        listenToInput = false;
    }

    private void OnEnable()
    {
        listenToInput = true;
    }
}
