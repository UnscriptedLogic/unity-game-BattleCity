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
            if (tankScore.score > ConnectionManager.instance.player.hiscore)
            {
                Debug.Log(tankScore.score);
                Debug.Log(ConnectionManager.instance.gameVariables.playerInfo.hiscore);

                ConnectionManager.instance.gameVariables.playerInfo.hiscore = tankScore.score;
                StartCoroutine(ConnectionManager.instance.UpdateScore(tankScore.score, (res) => { }));
            }
        };
    }
}