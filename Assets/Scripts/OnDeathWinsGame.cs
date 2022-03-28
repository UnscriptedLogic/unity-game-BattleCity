using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDeathWinsGame : MonoBehaviour
{
    public EntityHealth entityHealth;
    private GameManager manager;

    private void Start()
    {
        entityHealth = GetComponent<EntityHealth>();
        manager = GameManager.instance;
        if (manager == null)
        {
            Debug.LogWarning("The current scene does not have a valid game manager", gameObject);
            enabled = false;
            return;
        }
        entityHealth.onKilled += EntityHealth_onKilled;
    }

    private void EntityHealth_onKilled()
    {
        EndGame();
    }

    private void EndGame()
    {
        manager.GameOver();
    }
}
