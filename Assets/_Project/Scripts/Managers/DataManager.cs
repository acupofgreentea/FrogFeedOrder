using UnityEngine;
using UnityEngine.Events;

public static class DataManager 
{
    private static string _currentLevelKey = "CurrentLevel";
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(_currentLevelKey, 0);
        set => PlayerPrefs.SetInt(_currentLevelKey, value);
    }
    
    private static string _soundKey = "Sound";

    public static event UnityAction OnSoundChanged;
    public static bool Sound
    {
        get => PlayerPrefs.GetInt(_soundKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(_soundKey, value ? 1 : 0);
            OnSoundChanged?.Invoke();
        }
    }

    private static string _hapticKey = "Haptic";
    
    public static bool Haptic
    {
        get => PlayerPrefs.GetInt(_hapticKey, 1) == 1;
        set => PlayerPrefs.SetInt(_hapticKey, value ? 1 : 0);
    }
}
