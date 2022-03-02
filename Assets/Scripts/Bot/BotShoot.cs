using System.Collections;
using UnityEngine;

public class BotShoot : EntityShoot
{
    private BotManager botManager;
    public Transform shootAnchor;

    private IntervalShoot intervalShoot;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        botManager = manager as BotManager;
    }

    public override void SetDefaultBehaviour()
    {
        shootBehaviour = new IntervalShoot(botManager, transform, shootAnchor);
        intervalShoot = shootBehaviour as IntervalShoot;
        intervalShoot.onBulletShot += IntervalShoot_onBulletShot;
    }

    private void IntervalShoot_onBulletShot()
    {
        intervalShoot.ResetInterval(RandomValue.BetweenFloats(botManager.shootIntervals.x, botManager.shootIntervals.y));
    }

    private void Update()
    {
        intervalShoot.Shoot();
        intervalShoot.IntervalUpdate();
    }
}