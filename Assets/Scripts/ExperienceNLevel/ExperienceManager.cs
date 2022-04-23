using System;
using UnityEngine;

public static class ExperienceManager
{
    private static int currentExperience;
    private static int expCap = 1000;
    private static int currentLevel;

    public static int Experience { get => currentExperience; }
    public static int ExperienceCap { get => expCap; }
    public static int Level { get => currentLevel; }

    public static Action OnLevelUp;

    public static void SetExpLevel(int exp, int level)
    {
        currentExperience = exp;
        currentLevel = level;
    }

    public static void AddExperience(int amount)
    {
        currentExperience += amount;
        CheckLevelUp();

        UserManager.UpdateExpLevel(Experience, Level);
    }

    private static void CheckLevelUp()
    {
        if (currentExperience >= expCap)
        {
            int leftOver = currentExperience - expCap;
            currentExperience = leftOver;
            currentLevel++;

            CheckLevelUp();
            OnLevelUp?.Invoke();
        }

    }
}
