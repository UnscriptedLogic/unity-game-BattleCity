using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInput : Sephamore
{
    private Vector2 timeBetDecisions = new Vector2(1f, 2f);
    private float _decisionInterval;

    private MoveDecision[] moveDecisions;
    private BotManager botManager;

    public event Action<MovementState> moveStateUpdated;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);

        botManager = manager as BotManager;
        moveDecisions = botManager.moveDecisions;
        timeBetDecisions = botManager.decisionIntervals;
    }

    private void Update()
    {
        if (_decisionInterval <= 0)
        {
            RandomDirection();
            _decisionInterval = RandomValue.BetweenFloats(timeBetDecisions.x, timeBetDecisions.y);
        }
        else
        {
            _decisionInterval -= Time.deltaTime;
        }
    }

    private void RandomDirection()
    {
        float[] tierChances = new float[moveDecisions.Length];
        float prevChance = 0f;
        for (int i = 0; i < moveDecisions.Length; i++)
        {
            tierChances[i] = prevChance + moveDecisions[i].chance;
            prevChance = tierChances[i];
        }

        int randomTier = UnityEngine.Random.Range(0, 100);
        for (int i = 0; i < tierChances.Length; i++)
        {
            float highNum = i == tierChances.Length - 1 ? 100 : tierChances[i];
            float lowNum = i == 0 ? 0 : tierChances[i - 1];
            if (randomTier > lowNum && randomTier < highNum)
            {
                moveStateUpdated?.Invoke(moveDecisions[i].movementState);
                return;
            }
        }
    }
}
