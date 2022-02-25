using System.Collections;
using UnityEngine;

public class BotManager : TankManager
{
    [HideInInspector] public Vector2 shootIntervals;
    [HideInInspector] public Vector2 decisionIntervals;

    [HideInInspector] public MoveDecision[] moveDecisions;

    [Header("Enemy Settings")]
    public BotSettings enemySettings;

    protected override void InitSettings()
    {
        base.InitSettings();
        enemySettings = (BotSettings)entitySettings;
    }

    protected override void Initialize()
    {
        shootIntervals = enemySettings.shootInterval;
        decisionIntervals = enemySettings.decisionInterval;
        moveDecisions = enemySettings.moveDecisions;

        base.Initialize();
    }
}