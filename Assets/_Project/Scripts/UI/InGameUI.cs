using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingMovesText;
    [SerializeField] private TMP_Text levelText;

    private void Awake()
    {
        PlayerController.OnRemainingMovesUpdated += UpdateRemainingMoves;
    }

    private void Start()
    {
        levelText.text = $"Level: {DataManager.CurrentLevel + 1}";
    }

    private void OnDisable()
    {
        PlayerController.OnRemainingMovesUpdated -= UpdateRemainingMoves;
    }
    private void UpdateRemainingMoves(int remainingMoves)
    {
        remainingMovesText.text = $"Moves: {remainingMoves}";
    }
}
