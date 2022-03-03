using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Manager : MonoBehaviour
{
    public SemaphoreSlim gate = new SemaphoreSlim(1);

    public UnityEvent<Manager> BeforeInitialization;
    public UnityEvent<Manager, Semaphore> SephamoreInitialized;
    public UnityEvent<Manager> AfterInitialization;

    public Semaphore[] sephamores;

    protected async void InitializeSephamores()
    {
        BeforeInitialization?.Invoke(this);
        for (int i = 0; i < sephamores.Length; i++)
        {
            await gate.WaitAsync();
            sephamores[i].InitializeSephamore(this, () =>
            {
                SephamoreInitialized?.Invoke(this, sephamores[i]);
                gate.Release();
            });
        }
        AfterInitialization?.Invoke(this);
    }
}