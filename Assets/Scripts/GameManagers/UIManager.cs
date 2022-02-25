using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager
{
    private GameManager manager;
    public GameObject gameoverScreen;

    public override void Initialize()
    {
        manager = GameManager.instance;

        manager.onGameOver += DisplayGOScreen;
        gameoverScreen.SetActive(false);

        base.Initialize();
    }

    public void ToggleEffects()
    {
        manager.playSounds = !manager.playEffects;
        manager.playParticles = !manager.playEffects;
        manager.playEffects = !manager.playEffects;
    }

    public void ToggleSounds()
    {
        manager.playSounds = !manager.playSounds;
    }

    public void ToggleParticles()
    {
        manager.playParticles = !manager.playParticles;
    }

    private void DisplayGOScreen()
    {
        gameoverScreen.SetActive(true);
    }
}
