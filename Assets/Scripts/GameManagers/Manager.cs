using System;
using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public virtual void Initialize()
    {
        GameManager.instance.ReleaseGate(this);
    }
}