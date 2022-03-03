using System.Collections;
using UnityEngine;

public class BotManager : TankManager
{
    [HideInInspector] public Vector2 shootIntervals;
    [HideInInspector] public Vector2 decisionIntervals;
    [HideInInspector] public MoveDecision[] moveDecisions;

    private BotSettings botSettings;

    public override void InitializeEntity()
    {
        base.InitializeEntity();
        botSettings = settings as BotSettings;

        shootIntervals = botSettings.shootInterval;
        decisionIntervals = botSettings.decisionInterval;
        moveDecisions = botSettings.moveDecisions;
    }
}