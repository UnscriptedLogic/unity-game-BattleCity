using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStoreScore : MonoBehaviour
{
    public TankScore tankScore;

    private void Start()
    {
        GameManager.instance.onGameOver += delegate ()
        {
            if (GlobalVars.player != null)
            {
                if (tankScore.score > GlobalVars.player.hiscore)
                {
                    GlobalVars.SetPlayerScore(tankScore.score);
                    StartCoroutine(ConnectionManager.instance.UpdateScore((res) => { }));
                } 
            }
        };
    }
}