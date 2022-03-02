using System.Collections;
using UnityEngine;

public class BotManager : TankManager
{
    public Vector2 shootIntervals;
    public Vector2 decisionIntervals;
    public MoveDecision[] moveDecisions;

    private BotSettings botSettings;

    public override void InitializeEntity()
    {
        base.InitializeEntity();
        botSettings = entitySettings as BotSettings;

        shootIntervals = botSettings.shootInterval;
        decisionIntervals = botSettings.decisionInterval;
        moveDecisions = botSettings.moveDecisions;
    }
}