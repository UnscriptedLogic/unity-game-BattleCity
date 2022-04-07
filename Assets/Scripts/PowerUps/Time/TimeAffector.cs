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
    private EntityStateMachine entityStateMachine;

    private float originalSpeed;
    private float originalBulletSpeed;

    private GameObject particle;
    private bool activated;
    private float delay = 0.4f;
    private float check;
    private float checkInt = 0.1f;

    private void Start()
    {
        manager = GetComponent<TankManager>();
        movement = GetComponent<EntityMovement>();
        shoot = GetComponent<EntityShoot>();
        entityEffects = GetComponent<EntityEffects>();
        entityStateMachine = GetComponent<EntityStateMachine>();
    }

    private void Update()
    {
        if (delay <= 0f)
        {
            if (!activated)
            {
                Toggle(value: true);

                //Respawn
                PlayerRespawn playerRespawn = GetComponent<PlayerRespawn>();
                if (playerRespawn)
                {
                    playerRespawn.onPlayerRespawned += PlayerRespawn_onPlayerRespawned;
                }

                //State Machines
                if (entityStateMachine)
                {
                    entityStateMachine.enabled = false;
                }

                //VFX
                particle = Instantiate(AssetManager.instance.timeStopAuraIndividual, transform);
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
                {
                    particle.transform.position = hit.point;
                }

                BoxCollider boxCollider = transform.GetComponent<BoxCollider>();
                if (boxCollider)
                {
                    float scale = Mathf.Max(boxCollider.size.x, boxCollider.size.z) / 1.5f;
                    particle.transform.localScale = new Vector3(scale, 1f, scale);
                }

                activated = true;
            } else {
                //if (check <= 0f)
                //{
                //    //State Machines
                //    if (entityStateMachine)
                //    {
                //        entityStateMachine.enabled = false;
                //    }
                //    check = checkInt;
                //} else
                //{
                //    check -= Time.deltaTime;
                //}
            }
        } else
        {
            delay -= Time.deltaTime;
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

        if (entityStateMachine)
        {
            entityStateMachine.enabled = true;
        }

        Toggle(value: false);

        Destroy(particle);
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