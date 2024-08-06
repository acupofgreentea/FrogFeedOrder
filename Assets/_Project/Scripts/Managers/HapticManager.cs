using CandyCoded.HapticFeedback;
using UnityEngine;

public static class HapticManager
{
    public static void LightHaptic()
    {
        if (DataManager.Haptic == false)
            return;
        
        Debug.LogError("light haptic");
        HapticFeedback.LightFeedback();
    }
    
    public static void MediumHaptic()
    {
        if (DataManager.Haptic == false)
            return;
        
        Debug.LogError("medium haptic");
        HapticFeedback.MediumFeedback();
    }
    
    public static void HeavyHaptic()
    {
        if (DataManager.Haptic == false)
            return;
        
        Debug.LogError("heavy haptic");
        HapticFeedback.HeavyFeedback();
    }
}