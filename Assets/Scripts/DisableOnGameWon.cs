using System.Collections;
using UnityEngine;

public class DisableOnGameWon : MonoBehaviour
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

        manager.onGameWon += DisableSelf;
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
        manager.onGameWon -= DisableSelf;
    }
}