using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelHolderSO levelHolder;

    [SerializeField] private bool testLevel = false;

    [SerializeField, ShowIf("testLevel")] private LevelDataSO levelToTest;
    public static event UnityAction<LevelDataSO> OnLevelLoaded;
    public static event UnityAction<bool> OnLevelFinished;
    public static LevelManager Instance { get; private set; }

    private List<Frog> activeFrogs = new();

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        PlayerController.OnRemaningMovesFinished += OnRemainingMovesFinished;
        PlayerController.OnCollecterMoved += OnCollectorMoved;
        Frog.OnFrogSpawned += OnFrogSpawned;
    }
    private void OnDisable()
    {
        PlayerController.OnRemaningMovesFinished -= OnRemainingMovesFinished;
        PlayerController.OnCollecterMoved -= OnCollectorMoved;
        Frog.OnFrogSpawned -= OnFrogSpawned;
    }

    private void Start()
    {
        var levelData =  testLevel ? levelToTest : levelHolder.GetLevel(DataManager.CurrentLevel);
        if (levelData == null)
        {
            Debug.LogError("Level data not found!");
            return;
        }

        activeFrogs = new(levelData.FrogCount);
        Instantiate(levelData.LevelPrefab.gameObject);
        OnLevelLoaded?.Invoke(levelData);
    }
    
    public List<Frog> GetActiveFrogs() => activeFrogs;

    private void OnFrogSpawned(Frog frog)
    {
        activeFrogs.Add(frog);
    }

    private void OnCollectorMoved(bool isSuccess, ICollector collector)
    {
        if (!isSuccess)
            return;

        activeFrogs.Remove((Frog)collector);
        if (activeFrogs.Count == 0)
        {
            OnLevelFinished?.Invoke(true);
            DataManager.CurrentLevel++;
        }
    }


    private void OnRemainingMovesFinished()
    {
        if (activeFrogs.Count == 0)
            return;
        
        OnLevelFinished?.Invoke(false);
    }
}