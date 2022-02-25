﻿using System.Collections;
using UnityEngine;

public enum TimeModifyTypes
{
    Stop,
    Slow,
    SpeedUp
}

public class TimeAffector : MonoBehaviour
{
    public TimeModifyTypes timeModifyType;

    private TankManager manager;
    private EntityMovement movement;
    private EntityShoot shoot;

    private float originalSpeed;
    private float originalBulletSpeed;

    public float duration;

    private void Start()
    {
        manager = GetComponent<TankManager>();
        movement = GetComponent<EntityMovement>();
        shoot = GetComponent<EntityShoot>();

        Toggle(value: true);
    }

    private void Update()
    {
        if (duration <= 0)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        Toggle(value: false);
    }

    public void Toggle(bool value)
    {

        switch (timeModifyType)
        {
            case TimeModifyTypes.Stop:
                movement.enabled = !value;
                shoot.enabled = !value;
                break;
            case TimeModifyTypes.Slow:
                if (value)
                {
                    originalSpeed = manager.movementSpeed;
                    originalBulletSpeed = manager.bulletSpeed;

                    manager.movementSpeed /= 2f;
                    manager.bulletSpeed /= 2f;
                }
                else
                {
                    manager.movementSpeed = originalSpeed;
                    manager.bulletSpeed = originalBulletSpeed;
                }
                break;
            case TimeModifyTypes.SpeedUp:
                if (value)
                {
                    originalSpeed = manager.movementSpeed;
                    originalBulletSpeed = manager.bulletSpeed;

                    manager.movementSpeed *= 2f;
                    manager.bulletSpeed *= 2f;
                }
                else
                {
                    manager.movementSpeed = originalSpeed;
                    manager.bulletSpeed = originalBulletSpeed;
                }
                break;
            default:
                break;
        }
    }

    private void SaveStats(bool value)
    {
        if (value)
        {
            originalSpeed = manager.movementSpeed;
            originalBulletSpeed = manager.bulletSpeed;
        }
        else
        {
            manager.movementSpeed = originalSpeed;
            manager.bulletSpeed = originalBulletSpeed;
        }
    }
}