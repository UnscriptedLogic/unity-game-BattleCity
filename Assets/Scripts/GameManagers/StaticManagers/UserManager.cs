using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserManager
{
    public class User
    {
        public bool initialized;
        public int id;
        public string username;
        public string password;
        public int experience;
        public int level;
        public int highest_wave;

        public User(bool initialized = false, int id = 0, string username = "Unscripted_User1", string password = "")
        {
            this.initialized = initialized;
            this.id = id;
            this.username = username;
            this.password = password;
            experience = 0;
            level = 1;
            highest_wave = 0;
        }
    }

    public static User user;

    public static event Action onUserUpdated;

    public static void CreateUser(string name)
    {
        user = new User(initialized: true, username: name);
        onUserUpdated?.Invoke();
        SaveManager.Save();
    }

    public static void UpdateUsername(string name)
    {
        user.username = name;
        onUserUpdated?.Invoke();
        SaveManager.Save();
    }

    public static void UpdateWaveScore(int score)
    {
        user.highest_wave = score;
        onUserUpdated?.Invoke();
        SaveManager.Save();
    }

    public static void UpdateExpLevel(int exp, int lvl)
    {
        user.experience = exp;
        user.level = lvl;
        SaveManager.Save();
    }

    public static User GetUser()
    {
        if (user == null)
        {
            CreateUser("Unnamed_User");
        }

        return user;
    }
}
