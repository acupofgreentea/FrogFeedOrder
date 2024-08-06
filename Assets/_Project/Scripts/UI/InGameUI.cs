using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingMovesText;
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button hapticButton;
    [SerializeField] private Button soundButton;
    
    [SerializeField] private GameObject hapticOffIcon;
    [SerializeField] private GameObject soundOffIcon;
    

    private void Awake()
    {
        PlayerController.OnRemainingMovesUpdated += UpdateRemainingMoves;
        
        settingsButton.onClick.AddListener(HandleOnSettingsButtonClick);
        hapticButton.onClick.AddListener(HandleOnHapticButtonClick);
        soundButton.onClick.AddListener(HandleOnSoundButtonClick);
    }
    private void Start()
    {
        levelText.text = $"Level: {DataManager.CurrentLevel + 1}";
        
        hapticOffIcon.SetActive(!DataManager.Haptic);
        soundOffIcon.SetActive(!DataManager.Sound);
    }

    private void OnDisable()
    {
        PlayerController.OnRemainingMovesUpdated -= UpdateRemainingMoves;
    }
    
    private void HandleOnSettingsButtonClick()
    {
        hapticButton.gameObject.SetActive(!hapticButton.gameObject.activeInHierarchy);
        soundButton.gameObject.SetActive(!soundButton.gameObject.activeInHierarchy);
    }

    private void HandleOnSoundButtonClick()
    {
        DataManager.Sound = !DataManager.Sound;
        soundOffIcon.SetActive(!DataManager.Sound);
    }

    private void HandleOnHapticButtonClick()
    {
        DataManager.Haptic = !DataManager.Haptic;
        hapticOffIcon.SetActive(!DataManager.Haptic);
        
        HapticManager.LightHaptic();
    }
    private void UpdateRemainingMoves(int remainingMoves)
    {
        string text = remainingMoves == 0 ? "No Moves" : $"{remainingMoves} Moves";
        remainingMovesText.text = text;
    }
}
