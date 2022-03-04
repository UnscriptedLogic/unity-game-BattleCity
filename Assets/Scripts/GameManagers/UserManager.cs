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

        public User(bool initialized = false, int id = 0, string username = "Unscripted_User1", string password = "")
        {
            this.initialized = initialized;
            this.id = id;
            this.username = username;
            this.password = password;
        }
    }

    public static User user;
    public static int high_score = 0;

    public static event Action onUserUpdated;

    public static void CreateUser(User _user)
    {
        user = _user;
        onUserUpdated?.Invoke();
        SaveManager.Save();
    }

    public static void UpdateUsername(string name)
    {
        user.username = name;
        onUserUpdated?.Invoke();
        SaveManager.Save();
    }

    public static void UpdateScore(int score)
    {
        high_score = score;
        onUserUpdated?.Invoke();
        SaveManager.Save();
    }

    public static User GetUser()
    {
        if (user == null)
        {
            CreateUser(new User());
        }

        return user;
    }
}
