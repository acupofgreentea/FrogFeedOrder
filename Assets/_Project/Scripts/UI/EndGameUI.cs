using UnityEngine;
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
        TransitionManager.Instance.Fade(2f, () =>
        {
            if (isWin)
                winPanel.SetActive(true);
            else
                failPanel.SetActive(true); 
        });
    }

    private void OnTryAgainButtonClicked()
    {
        SceneController.Instance.LoadSceneAsync(Constants.GAME_SCENE_INDEX, 1f,
            () =>
            {
                TransitionManager.Instance.Fade(2f);

            });
    }

    private void OnContinueButtonClicked()
    {
        SceneController.Instance.LoadSceneAsync(Constants.GAME_SCENE_INDEX, 1f,
        () =>
        {
            TransitionManager.Instance.Fade(2f);

        });
    }
}
