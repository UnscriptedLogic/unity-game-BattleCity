using System.Collections;
using UnityEngine;

public class Stun : Affectors
{
    private EntityMovement entityMovement;
    private EntityShoot entityShoot;

    private void Start()
    {
        if (TargetAlreadyAffected(out Stun stun))
        {
            stun.Activate(duration);
            Destroy(this);
            return;
        }
    }

    public override void Activate(float duration)
    {
        this.duration = duration;
        ToggleBehaviours(false);
    }

    private void Update()
    {
        if (duration < 0f)
        {
            ToggleBehaviours(true);
            Destroy(this);
        } else
        {
            duration -= Time.deltaTime;
        }
    }

    private void ToggleBehaviours(bool value)
    {
        if (entityMovement) entityMovement.enabled = value;
        if (entityShoot) entityShoot.enabled = value;
    }
}