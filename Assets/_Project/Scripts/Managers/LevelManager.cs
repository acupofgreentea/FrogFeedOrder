using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelHolderSO levelHolder;

    [SerializeField] private bool testLevel = false;

    [SerializeField, ShowIf("testLevel")] private int levelTestIndex = 0;

    public static event UnityAction<LevelDataSO> OnLevelLoaded;
    public static event UnityAction<bool> OnLevelFinished;

    private int levelFrogCount;

    private void Awake()
    {
        PlayerController.OnRemaningMovesFinished += OnRemainingMovesFinished;
        PlayerController.OnCollecterMoved += OnCollectorMoved;
    }

    private void OnCollectorMoved(bool isSuccess)
    {
        levelFrogCount--;
        if (levelFrogCount == 0)
        {
            OnLevelFinished?.Invoke(true);
            Debug.Log("Level Completed!");
        }
    }

    private void OnDisable()
    {
        PlayerController.OnRemaningMovesFinished -= OnRemainingMovesFinished;
    }

    private void OnRemainingMovesFinished()
    {
        OnLevelFinished?.Invoke(levelFrogCount == 0);
    }

    private void Start()
    {
        int levelIndex = testLevel ? levelTestIndex : DataManager.CurrentLevel;
        
        var levelData =  levelHolder.GetLevel(levelIndex);
        if (levelData == null)
        {
            Debug.LogError("Level data not found!");
            return;
        }
        
        levelFrogCount = levelData.FrogCount;
        OnLevelLoaded?.Invoke(levelData);
    }
}