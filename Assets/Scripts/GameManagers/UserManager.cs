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

    public static void CreatePlayer(string username)
    {
        user = new User(initialized: true, username: username);
    }
}
