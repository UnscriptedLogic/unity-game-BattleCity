using System.Collections;
using UnityEngine;

public class BotShoot : EntityShoot
{

    private BotManager botManager;
    public Transform bulletAnchor;
    private IntervalShoot intervalShoot;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        botManager = manager as BotManager;
        shootAnchor = bulletAnchor;
    }

    public override void SetDefaultBehaviour()
    {
        shootBehaviour = new IntervalShoot(tankManager: botManager, entityShoot: this, transform: transform);
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