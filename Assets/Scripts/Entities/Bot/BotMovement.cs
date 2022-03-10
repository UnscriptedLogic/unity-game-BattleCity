using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : EntityMovement
{
    public Rigidbody rb;
    public RandomBotInput botInput;
    private BotManager botManager;

    protected override void SephamoreStart(Manager manager)
    {
        botManager = manager as BotManager; 
        base.SephamoreStart(manager);
    }

    public override void SetDefaultBehaviour()
    {
        movementBehaviour = new BotRandomDirectionMovement(botManager, botInput, rb, transform);
    }

    private void FixedUpdate()
    {
        movementBehaviour.Move();
    }
}
