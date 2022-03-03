using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : TankManager
{
    public PlayerInput playerInput;
    public Rigidbody rb;

    public override void InitializeEntity()
    {
        base.InitializeEntity();
    }
}