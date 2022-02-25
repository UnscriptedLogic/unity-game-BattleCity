using System.Collections;
using UnityEngine;

public class DeathEndsGame : MonoBehaviour
{
    public EntityHealth entityHealth;

    private GameManager manager;

    private void Start()
    {
        manager = GameManager.instance;
        if (manager == null)
        {
            Debug.LogWarning("The current scene does not have a valid game manager", gameObject);
            enabled = false;
            return;
        }

        entityHealth.onKilled += EntityHealth_onKilled; ;
    }

    private void EntityHealth_onKilled(EntityManager obj)
    {
        EndGame();
    }

    private void EndGame()
    {
        manager.GameOver();
    }
}