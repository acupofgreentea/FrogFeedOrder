using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class ToolbarUtilities
{
    private static readonly string loadingScenePath = "Assets/_Project/Scenes/LoadingScene.unity";
    private static readonly string gameScenePath = "Assets/_Project/Scenes/GameScene.unity";

    private static readonly SceneAsset loadingScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(loadingScenePath);
    private static readonly SceneAsset gameScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(gameScenePath);

    static ToolbarUtilities()
    {
        UnityToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        EditorSceneManager.playModeStartScene = null;
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("L", "Switch to LoadingScene"), ToolbarStyles.commandButtonStyle))
        {
            if (SceneManager.GetActiveScene().name == loadingScene.name)
                return;
            
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(loadingScenePath);
            }
        }

        if (GUILayout.Button(new GUIContent("G", "Switch to GameScene"), ToolbarStyles.commandButtonStyle))
        {
            if (SceneManager.GetActiveScene().name == gameScene.name)
                return;
            
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(gameScenePath);
            }
        }

        if (GUILayout.Button(new GUIContent("P", "Start from loading scene"), ToolbarStyles.commandButtonStyle))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.playModeStartScene = loadingScene;
                EditorApplication.EnterPlaymode();
            }
        }
    }
}

static class ToolbarStyles
{
    public static readonly GUIStyle commandButtonStyle;

    static ToolbarStyles()
    {
        commandButtonStyle = new GUIStyle("Command")
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold
        };
    }
}
