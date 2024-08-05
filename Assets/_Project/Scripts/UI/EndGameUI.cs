using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failPanel;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button tryAgainButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        
        LevelManager.OnLevelFinished += OnLevelFinished;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelFinished -= OnLevelFinished;
    }

    private void OnLevelFinished(bool isWin)
    {
        if (isWin)
            winPanel.SetActive(true);
        else
            failPanel.SetActive(true);
    }

    private void OnTryAgainButtonClicked()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnContinueButtonClicked()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
