using System.Collections;
using UnityEngine;

public enum BulletType
{
    Normal,
    Piercing
}

public class BulletMovement : EntityMovement
{
    private BulletManager bulletManager;
    public Rigidbody rb;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        bulletManager = manager as BulletManager;
    }

    public override void SetDefaultBehaviour()
    {
        movementBehaviour = new HeadlessLinearMovement(bulletManager, rb, transform);
    }

    private void FixedUpdate()
    {
        movementBehaviour.Move();
    }
}