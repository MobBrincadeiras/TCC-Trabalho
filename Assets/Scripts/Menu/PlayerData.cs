using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    private const string MONEY_KEY = "PlayerMoney";
    private const string LEVEL_KEY = "PlayerLevel";
    private const string EXP_KEY = "PlayerEXP";
    private const string TOTAL_EXP_KEY = "PlayerTotalEXP";
    private const string PONG_RECORD_KEY = "PongRecord";

    #if !UNITY_EDITOR
    private static readonly Dictionary<string, int> temporaryData = new Dictionary<string, int>();
    #endif

    private static int GetData(string key, int defaultValue = 0)
    {
        #if UNITY_EDITOR
        return PlayerPrefs.GetInt(key, defaultValue);
        #else
        return temporaryData.TryGetValue(key, out int value)
            ? value
            : defaultValue;
        #endif
    }

    private static void SetData(string key, int value)
    {
        #if UNITY_EDITOR
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
        #else
        temporaryData[key] = value;
        #endif
    }

    public static int Money => GetData(MONEY_KEY, 0);

    public static int Level => GetData(LEVEL_KEY, 1);

    public static int Experience => GetData(EXP_KEY, 0);

    public static int TotalExperience => GetData(TOTAL_EXP_KEY, 0);

    public static int PongRecord => GetData(PONG_RECORD_KEY, 0);

    public static int ExperienceToNextLevel =>
        100 + (Level - 1) * 50;

    public static void AddMoney(int amount)
    {
        int newMoney = Mathf.Max(0, Money + amount);
        SetData(MONEY_KEY, newMoney);
    }

    public static bool TrySpendMoney(int amount)
    {
        if (amount < 0 || Money < amount)
            return false;

        AddMoney(-amount);
        return true;
    }

    public static bool IsClothingUnlocked(string category, int index)
    {
        if (index == 0)
            return true;

        string key = $"Clothing_{category}_{index}";

        return GetData(key, 0) == 1;
    }

    public static void UnlockClothingItem(string category, int index)
    {
        string key = $"Clothing_{category}_{index}";
        SetData(key, 1);
    }

    public static void SetEquippedClothing(string category, int index)
    {
        string key = $"Equipped_{category}";
        SetData(key, index);
    }

    public static int GetEquippedClothing(string category)
    {
        string key = $"Equipped_{category}";
        return GetData(key, 0);
    }

    public static void AddExperience(int amount)
    {
        if (amount <= 0)
            return;

        int currentExp = Experience + amount;
        int currentLevel = Level;
        int totalExp = TotalExperience + amount;

        while (currentExp >= 100 + (currentLevel - 1) * 50)
        {
            currentExp -= 100 + (currentLevel - 1) * 50;
            currentLevel++;
        }

        SetData(LEVEL_KEY, currentLevel);
        SetData(EXP_KEY, currentExp);
        SetData(TOTAL_EXP_KEY, totalExp);
    }

    public static void UpdatePongRecord(int score)
    {
        if (score > PongRecord)
        {
            SetData(PONG_RECORD_KEY, score);
        }
    }
}
