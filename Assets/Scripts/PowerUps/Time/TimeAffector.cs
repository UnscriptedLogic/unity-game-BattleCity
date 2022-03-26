using System.Collections;
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
    private EntityEffects entityEffects;

    private float originalSpeed;
    private float originalBulletSpeed;

    public float duration;

    private void Start()
    {
        manager = GetComponent<TankManager>();
        movement = GetComponent<EntityMovement>();
        shoot = GetComponent<EntityShoot>();
        entityEffects = GetComponent<EntityEffects>();

        Toggle(value: true);


        PlayerRespawn playerRespawn = GetComponent<PlayerRespawn>();
        if (playerRespawn)
        {
            playerRespawn.onPlayerRespawned += PlayerRespawn_onPlayerRespawned;
        }
    }

    private void PlayerRespawn_onPlayerRespawned()
    {
        Toggle(value: true);
    }

    private void OnDestroy()
    {
        PlayerRespawn playerRespawn = GetComponent<PlayerRespawn>();
        if (playerRespawn)
        {
            playerRespawn.onPlayerRespawned -= PlayerRespawn_onPlayerRespawned;
        }
        Toggle(value: false);
    }

    public void Toggle(bool value)
    {

        switch (timeModifyType)
        {
            case TimeModifyTypes.Stop:
                if (movement) movement.enabled = !value;
                if (shoot) shoot.enabled = !value;
                if (entityEffects) entityEffects.enabled = !value;
                break;
            case TimeModifyTypes.Slow:
                if (value)
                {
                    originalSpeed = manager.speed;
                    originalBulletSpeed = manager.bulletSpeed;

                    manager.speed /= 2f;
                    manager.bulletSpeed /= 2f;
                }
                else
                {
                    manager.speed = originalSpeed;
                    manager.bulletSpeed = originalBulletSpeed;
                }
                break;
            case TimeModifyTypes.SpeedUp:
                if (value)
                {
                    originalSpeed = manager.speed;
                    originalBulletSpeed = manager.bulletSpeed;

                    manager.speed *= 2f;
                    manager.bulletSpeed *= 2f;
                }
                else
                {
                    manager.speed = originalSpeed;
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
            originalSpeed = manager.speed;
            originalBulletSpeed = manager.bulletSpeed;
        }
        else
        {
            manager.speed = originalSpeed;
            manager.bulletSpeed = originalBulletSpeed;
        }
    }
}