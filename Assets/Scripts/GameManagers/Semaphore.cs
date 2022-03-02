using System;
using System.Collections;
using UnityEngine;

public class Semaphore : MonoBehaviour
{
    public void InitializeSephamore(Manager initManager, Action callback)
    {
        SephamoreStart(initManager);
        callback();
    }

    protected virtual void SephamoreStart(Manager manager)
    {
        //Used to replace the start methods
    }
}