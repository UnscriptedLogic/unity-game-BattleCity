using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Semaphore
{
    private GameManager manager;
    public GameObject gameoverScreen;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        this.manager = GameManager.instance;

        this.manager.onGameOver += DisplayGOScreen;
        gameoverScreen.SetActive(false);
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
