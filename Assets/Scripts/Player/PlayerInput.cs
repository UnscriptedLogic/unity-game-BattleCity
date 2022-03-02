using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EventType
{
    Performed,
    Cancelled,
    Started
}

public enum ActionType
{
    Move,
    Shoot
}

public class PlayerInput : Sephamore
{
    public InputActionAsset actionAsset;
    private InputAction move, shoot;

    protected override void SephamoreStart(Manager manager)
    {
        InputActionMap actionMap = actionAsset.FindActionMap("Default");
        move = actionMap.FindAction("Move");
        shoot = actionMap.FindAction("Shoot");

        move.Enable();
        shoot.Enable();
    }

    public void RegisterBind(Action<InputAction.CallbackContext> method, ActionType actionType, EventType eventType)
    {
        InputAction action = GetBindingAction(actionType);
        switch (eventType)
        {
            case EventType.Performed:
                action.performed += method;
                break;
            case EventType.Cancelled:
                action.canceled += method;
                break;
            case EventType.Started:
                action.started += method;
                break;
            default:
                Debug.Log("The stated event type was not found. Using PERFORMED instead", gameObject);
                action.performed += method;
                break;
        }
    }

    public InputAction GetBindingAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Move:
                return move;
            case ActionType.Shoot:
                return shoot;
            default:
                Debug.Log("The action did not exist in the enums.", gameObject);
                return null;
        }
    }
}