using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHolder", menuName = "LevelHolder")]
public class LevelHolderSO : ScriptableObject
{
    [SerializeField] private List<LevelDataSO> levels;

    public LevelDataSO GetLevel(int level)
    {
        return level < levels.Count ? levels[level] : null;
    }
}