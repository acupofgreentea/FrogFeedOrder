using UnityEditor;
using UnityEngine;

public static class AddLevelContextMenu
{
    [MenuItem("Assets/Add to Level Holder", true)]
    private static bool ValidateSelection()
    {
        return Selection.activeObject is LevelDataSO;
    }

    [MenuItem("Assets/Add to Level Holder")]
    private static void AddToLevelHolder()
    {
        LevelDataSO selectedObject = Selection.activeObject as LevelDataSO;
        AddToLevelHolder(selectedObject);
    }

    [MenuItem("Assets/Remove Level")]
    private static void RemoveLevel()
    {
        LevelDataSO selectedObject = Selection.activeObject as LevelDataSO;
        RemoveFromLevelHolderAndDelete(selectedObject);
    }

    public static void AddToLevelHolder(LevelDataSO levelDataSo)
    {
        LevelHolderSO levelHolderSo = Helpers.FindObject<LevelHolderSO>();

        if (levelHolderSo != null)
        {
            levelHolderSo.AddLevel(levelDataSo);
            EditorUtility.SetDirty(levelHolderSo);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogError("LevelHolderSO not found.");
        }
    }

    private static void RemoveFromLevelHolderAndDelete(LevelDataSO levelDataSo)
    {
        LevelHolderSO levelHolderSo = Helpers.FindObject<LevelHolderSO>();
        if (levelHolderSo == null)
        {
            Debug.LogError("LevelHolderSO not found.");
            return;
        }

        if (levelHolderSo.RemoveLevel(levelDataSo))
        {
            string assetPath = AssetDatabase.GetAssetPath(levelDataSo);

            if (string.IsNullOrEmpty(assetPath))
                return;

            var levelPrefab = levelDataSo.LevelPrefab;
            if (levelPrefab != null)
            {
                string prefabPath = AssetDatabase.GetAssetPath(levelPrefab);
                AssetDatabase.DeleteAsset(prefabPath);
            }
            AssetDatabase.DeleteAsset(assetPath);

            EditorUtility.SetDirty(levelHolderSo);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogWarning($"{levelDataSo.name} is not in the Level Holder.");
        }
    }

    
}