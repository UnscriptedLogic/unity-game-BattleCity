using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : Semaphore
{
    private GameManager manager;
    public GameObject gameoverScreen;
    public GameObject gamewinScreen;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        this.manager = GameManager.instance;

        this.manager.onGameOver += DisplayGOScreen;
        this.manager.onGameWon += DisplayGWScreen;
        gameoverScreen.SetActive(false);
        gamewinScreen.SetActive(false);
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

    private void DisplayGWScreen()
    {
        gamewinScreen.SetActive(true);
    }
}
