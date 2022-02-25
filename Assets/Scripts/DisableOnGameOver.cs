using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGameOver : MonoBehaviour
{
    private GameManager manager;
    [Tooltip("Enable this to fully destroy the object instead of just disabling it.")]
    public bool destroy;

    private void Start()
    {
        manager = GameManager.instance;
        if (!manager)
        {
            return;
        }

        manager.onGameOver += DisableSelf;
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        manager.onGameOver -= DisableSelf;
    }
}
