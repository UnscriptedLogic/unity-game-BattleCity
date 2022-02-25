using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInput : EntitySemaphore
{
    private Vector2 timeBetDecisions = new Vector2(1f, 2f);
    private float _decisionInterval;

    private MoveDecision[] moveDecisions;
    public BotMovement enemyMovement;
    public BotManager enemyManager;

    public override void Initialize(EntityManager manager)
    {
        moveDecisions = enemyManager.moveDecisions;
        timeBetDecisions = enemyManager.decisionIntervals;

        base.Initialize(manager);
    }

    private void Update()
    {
        if (_decisionInterval <= 0)
        {
            RandomDirection();
            _decisionInterval = RandomValue.BetweenFloats(timeBetDecisions.x, timeBetDecisions.y);
        } else
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

        int randomTier = Random.Range(0, 100);
        for (int i = 0; i < tierChances.Length; i++)
        {
            float highNum = i == tierChances.Length - 1 ? 100 : tierChances[i];
            float lowNum = i == 0 ? 0 : tierChances[i - 1];
            if (randomTier > lowNum && randomTier < highNum)
            {
                enemyMovement.SetMovementState(moveDecisions[i].movementState);
                return;
            }
        }
    }
}
