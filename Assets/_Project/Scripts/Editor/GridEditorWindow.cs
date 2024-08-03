using UnityEngine;
using UnityEditor;

public class GridEditorWindow : EditorWindow
{
    private int width = 5;
    private int height = 5;
    private int[,] gridValues;
    private Vector2 scrollPos;
    private LevelDataSO _levelDataSo;

    [MenuItem("Window/Custom Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditorWindow>("Grid Editor");
    }

    private void OnEnable()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        gridValues = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gridValues[x, y] = 1;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);

        int newWidth = EditorGUILayout.IntField("Width", width);
        int newHeight = EditorGUILayout.IntField("Height", height);

        if (newWidth != width || newHeight != height)
        {
            width = newWidth;
            height = newHeight;
            InitializeGrid();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int y = 0; y < height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < width; x++)
            {
                DrawGridValue(x, y);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Grid Data"))
        {
            SaveGridData();
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawGridValue(int x, int y)
    {
        Rect buttonRect = GUILayoutUtility.GetRect(30, 30, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && buttonRect.Contains(currentEvent.mousePosition))
        {
            if (currentEvent.button == 0)
            {
                gridValues[x, y]++;
                currentEvent.Use();
            }
            else if (currentEvent.button == 1)
            {
                gridValues[x, y] = Mathf.Max(0, gridValues[x, y] - 1);
                currentEvent.Use();
            }
        }

        GUI.Button(buttonRect, gridValues[x, y].ToString());
    }

    private void SaveGridData()
    {
        string fileName = "Level_";
        string extension = ".asset";
        string directory = "Assets/_Project/ScriptableObjects/Levels/";

        int index = 1;
        string path = $"{directory}{fileName}{index}{extension}";

        while (System.IO.File.Exists(path))
        {
            path = $"{directory}{fileName}{index}{extension}";
            index++;
        }

        _levelDataSo = CreateInstance<LevelDataSO>();
        AssetDatabase.CreateAsset(_levelDataSo, path);

        _levelDataSo.Initialize(width, height, gridValues);
        EditorUtility.SetDirty(_levelDataSo); 
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(); 

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = _levelDataSo;
    }


}
