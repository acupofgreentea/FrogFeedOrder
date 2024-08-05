using UnityEditor;

public static class AddLevelContextMenu
{
    [MenuItem("Assets/Add to Level Holder", true)]
    private static bool ValidateSelection()
    {
        return Selection.activeObject is LevelDataSO;
    }

    [MenuItem("Assets/Add to Level Holder")]
    private static void CustomAction()
    {
        LevelDataSO selectedObject = Selection.activeObject as LevelDataSO;

        LevelHolderSO levelHolderSo = GetLevelHolderSO();
        levelHolderSo.AddLevel(selectedObject);
    }

    private static LevelHolderSO GetLevelHolderSO()
    {
        string typeName = nameof(LevelHolderSO);

        string[] guids = AssetDatabase.FindAssets($"t:{typeName}");

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        LevelHolderSO levelHolderSo = AssetDatabase.LoadAssetAtPath<LevelHolderSO>(path);

        return levelHolderSo != null ? levelHolderSo : null;
    }
}