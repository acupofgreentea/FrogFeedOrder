using UnityEngine;

public static class DataManager 
{
    private static string _currentLevelKey = "CurrentLevel";
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(_currentLevelKey, 0);
        set => PlayerPrefs.SetInt(_currentLevelKey, value);
    }
}
