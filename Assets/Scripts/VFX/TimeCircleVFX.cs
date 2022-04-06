using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeCircleVFX : MonoBehaviour
{
    public float pauseDelay = 1f;
    private float currTimer;

    [GradientUsage(true)]
    public Gradient gradient;

    public ParticleSystem[] particleSystems;

    private void Start()
    {
        currTimer = pauseDelay;
        
    }

    private void UpdateColour()
    {
        float increment = 1f / particleSystems.Length;
        float eval = 0f;
        ForEach(partSystem =>
        {
            ParticleSystem.MainModule main = partSystem.main;
            main.startColor = gradient.Evaluate(eval);

            partSystem.Clear();
            partSystem.Play();
            eval += increment;
        });
    }

    private void ForEach(Action<ParticleSystem> method)
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            method(particleSystems[i]);
        }
    }

    private void OnValidate()
    {
        UpdateColour();
    }
}
