using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static Player player;
    public static event Action onPlayerUpdated;

    public static void SetEmptyPlayer()
    {
        player = new Player();
        onPlayerUpdated?.Invoke();
    }

    public static Player GetPlayer()
    {
        if (player == null)
        {
            player = new Player();
        }

        onPlayerUpdated?.Invoke();
         
        return player;
    }

    public static void SetPlayer(Player _player)
    {
        player = _player;
        onPlayerUpdated?.Invoke();
    }

    public static void SetPlayerScore(int amount)
    {
        player.hiscore = amount;
    }

    public static void UpdatePlayer(int id, string username, string password, int hiscore)
    {
        player.initialized = true;
        player.id = id;
        player.username = username;
        player.password = password;
        player.hiscore = hiscore;
        onPlayerUpdated?.Invoke();
    }

    public static void PlayerUpdated()
    {
        onPlayerUpdated?.Invoke();
    }
}
