using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class GridEditorWindow : EditorWindow
{
    private int width = 4;
    private int depth = 4;
    private int totalMovesCount = 5;
    private GridCellData[,] gridValues;
    private Vector2 scrollPos;
    private LevelDataSO gridData;
    private bool editLevel = false;

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditorWindow>("Level Editor");
    }

    private void OnEnable()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        if (editLevel && gridData != null)
        {
            width = gridData.Width;
            depth = gridData.Depth;
            gridValues = gridData.GetGridValues();
        }
        else
        {
            gridValues = new GridCellData[width, depth];

            for (int y = 0; y < depth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    gridValues[x, y] = new GridCellData(1);
                }
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);

        int newWidth = EditorGUILayout.IntField("Width", width, GUILayout.Width(200));
        int newHeight = EditorGUILayout.IntField("Depth", depth, GUILayout.Width(200));
        totalMovesCount = EditorGUILayout.IntField("Total Moves", totalMovesCount, GUILayout.Width(200));
        editLevel = EditorGUILayout.Toggle("Edit Level", editLevel);
        if (editLevel)
        {
            LevelDataSO previousGridData = gridData;
            gridData = (LevelDataSO)EditorGUILayout.ObjectField("Level Data", gridData, typeof(LevelDataSO), false,
                GUILayout.Width(400));

            if (gridData != previousGridData)
            {
                InitializeGrid();
            }
        }
        else
        {
            if (gridData != null)
            {
                gridData = null;
                InitializeGrid();
            }
        }

        GUILayout.Space(20);

        if (newWidth != width || newHeight != depth)
        {
            width = newWidth;
            depth = newHeight;
            InitializeGrid();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int y = 0; y < depth; y++)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(10);
            for (int x = 0; x < width; x++)
            {
                DrawGridCell(x, y);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Create Level"))
        {
            CreateLevel();
        }
    }


    private void DrawGridCell(int x, int y)
    {
        float defaultWidth = 120f;
        float maxWidth = 200f;
        bool shouldExpand = false;

        for (int col = 0; col < width; col++)
        {
            for (int h = 0; h < gridValues[col, y].height; h++)
            {
                if (gridValues[col, y].states[h] != GridState.Empty)
                {
                    shouldExpand = true;
                    break;
                }
            }

            if (shouldExpand) break;
        }

        float cellWidth = shouldExpand ? maxWidth : defaultWidth;

        EditorGUILayout.BeginVertical("box", GUILayout.Width(cellWidth), GUILayout.Height(80));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            if (gridValues[x, y].height < 10)
            {
                gridValues[x, y].SetHeight(gridValues[x, y].height + 1);
            }
        }

        GUILayout.Label(gridValues[x, y].height.ToString(), GUILayout.Width(20));

        if (GUILayout.Button("-", GUILayout.Width(20)) && gridValues[x, y].height > 1)
        {
            gridValues[x, y].SetHeight(gridValues[x, y].height - 1);
        }

        EditorGUILayout.EndHorizontal();

        for (int h = 0; h < gridValues[x, y].height; h++)
        {
            bool showColorPicker = gridValues[x, y].states[h] != GridState.Empty;
            bool showDirection = gridValues[x, y].states[h] == GridState.Frog ||
                                 gridValues[x, y].states[h] == GridState.DirectionChanger;
            EditorGUILayout.BeginHorizontal();
            gridValues[x, y].states[h] =
                (GridState)EditorGUILayout.EnumPopup(gridValues[x, y].states[h], GUILayout.Width(60));

            if (showColorPicker)
            {
                if (gridValues[x, y].colors.Length <= h)
                    gridValues[x, y].colors = gridValues[x, y].colors.Concat(new ContentColor[1]).ToArray();

                gridValues[x, y].colors[h] =
                    (ContentColor)EditorGUILayout.EnumPopup(gridValues[x, y].colors[h], GUILayout.Width(60));
            }

            if (showDirection)
            {
                if (gridValues[x, y].directions.Length <= h)
                    gridValues[x, y].directions = gridValues[x, y].directions.Concat(new Direction[1]).ToArray();

                gridValues[x, y].directions[h] =
                    (Direction)EditorGUILayout.EnumPopup(gridValues[x, y].directions[h], GUILayout.Width(60));
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void CreateLevel()
    {
        var creator = FindObjectOfType<GridCreator>();
        
        if(creator == null)
        {
            Debug.LogWarning("No GridCreator found in the scene. Creating one.");
            var g = new GameObject("GridCreator");
            g.AddComponent<GridCreator>();
            return;
        }

        int frogCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                for (int h = 0; h < gridValues[i, j].height; h++)
                {
                    if (gridValues[i, j].states[h] == GridState.Frog)
                    {
                        frogCount++;
                    }
                }
            }
        }

        string levelFileName = "Level_";
        string levelFileExtension = ".asset";
        string levelDirectory = "Assets/_Project/ScriptableObjects/Levels/";
        string prefabDirectory = "Assets/_Project/Prefabs/Levels/";
        

        if (!Directory.Exists(prefabDirectory))
        {
            Directory.CreateDirectory(prefabDirectory);
        }

        int index = 1;
        string levelPath = $"{levelDirectory}{levelFileName}{index}{levelFileExtension}";

        while (File.Exists(levelPath))
        {
            levelPath = $"{levelDirectory}{levelFileName}{index}{levelFileExtension}";
            index++;
        }
        
        GameObject level = new GameObject();
        level.AddComponent<GridManager>();

        if (editLevel)
        {
            if (gridData == null)
            {
                Debug.LogWarning("No LevelDataSO selected for saving.");
                return;
            }

            gridData.Initialize(width, depth, totalMovesCount, frogCount, gridValues);
            creator.CreateGridFromDataEditor(gridData, level.transform);
            level.name = gridData.name;
            level.GetComponent<GridManager>().CopyCells(creator.GetCells);
        }
        else
        {
            LevelDataSO newLevelData = CreateInstance<LevelDataSO>();
            AssetDatabase.CreateAsset(newLevelData, levelPath);
            EditorUtility.SetDirty(newLevelData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            newLevelData.Initialize(width, depth, totalMovesCount, frogCount, gridValues);
            level.name = newLevelData.name;
            creator.CreateGridFromDataEditor(newLevelData, level.transform);
            level.GetComponent<GridManager>().CopyCells(creator.GetCells);
        }

        string prefabPath = Path.Combine(prefabDirectory, level.name + ".prefab");
        GameObject newPrefab = null;

        if (File.Exists(prefabPath))
        {
            if (EditorUtility.DisplayDialog("Prefab Already Exists",
                    "A prefab with this name already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                newPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(level, prefabPath, InteractionMode.UserAction);
            }
        }
        else
        {
            newPrefab = PrefabUtility.SaveAsPrefabAsset(level, prefabPath);
        }

        if (editLevel)
        {
            gridData.LevelPrefab = newPrefab.GetComponent<GridManager>();
            EditorUtility.SetDirty(gridData);
        }
        else
        {
            LevelDataSO newLevelData = AssetDatabase.LoadAssetAtPath<LevelDataSO>(levelPath);
            newLevelData.LevelPrefab = newPrefab.GetComponent<GridManager>();
            AddLevelContextMenu.AddToLevelHolder(newLevelData);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Prefab created at {prefabPath}");
    }
}