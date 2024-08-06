using CandyCoded.HapticFeedback;
using UnityEngine;

public static class HapticManager
{
    public static void LightHaptic()
    {
        if (DataManager.Haptic == false)
            return;
        
        Debug.Log("light haptic");
        HapticFeedback.LightFeedback();
    }
    
    public static void MediumHaptic()
    {
        if (DataManager.Haptic == false)
            return;
        
        Debug.Log("medium haptic");
        HapticFeedback.MediumFeedback();
    }
    
    public static void HeavyHaptic()
    {
        if (DataManager.Haptic == false)
            return;
        
        Debug.Log("heavy haptic");
        HapticFeedback.HeavyFeedback();
    }
}