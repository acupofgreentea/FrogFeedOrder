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

        editLevel = EditorGUILayout.Toggle("Edit Level", editLevel);

        if (editLevel)
        {
            LevelDataSO previousGridData = gridData;
            gridData = (LevelDataSO)EditorGUILayout.ObjectField("Level Data", gridData, typeof(LevelDataSO), false, GUILayout.Width(400));

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

        int newWidth = EditorGUILayout.IntField("Width", width, GUILayout.Width(200));
        int newHeight = EditorGUILayout.IntField("Depth", depth, GUILayout.Width(200));
        totalMovesCount = EditorGUILayout.IntField("Total Moves", totalMovesCount, GUILayout.Width(200));

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

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel();
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
            bool showDirection = gridValues[x, y].states[h] == GridState.Frog || gridValues[x, y].states[h] == GridState.DirectionChanger;
            EditorGUILayout.BeginHorizontal();
            gridValues[x, y].states[h] =
                (GridState)EditorGUILayout.EnumPopup(gridValues[x, y].states[h], GUILayout.Width(60));

            if (showColorPicker)
            {
                if(gridValues[x, y].colors.Length <= h)
                    gridValues[x, y].colors = gridValues[x, y].colors.Concat(new ContentColor[1]).ToArray();
                
                gridValues[x, y].colors[h] = (ContentColor)EditorGUILayout.EnumPopup(gridValues[x, y].colors[h], GUILayout.Width(60));
            }

            if (showDirection)
            {
                if(gridValues[x, y].directions.Length <= h)
                    gridValues[x, y].directions = gridValues[x, y].directions.Concat(new Direction[1]).ToArray();
                
                gridValues[x, y].directions[h] = (Direction)EditorGUILayout.EnumPopup(gridValues[x, y].directions[h], GUILayout.Width(60));
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }


    private void SaveLevel()
    {
        int frogCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                for (int h = 0; h < gridValues[i, j].height; h++)
                {
                    if(gridValues[i, j].states[h] == GridState.Frog)
                        frogCount++;
                }
            }
        }
        
        if (editLevel)
        {
            if (gridData == null)
            {
                Debug.LogWarning("No LevelDataSO selected for saving.");
                return;
            }

            gridData.Initialize(width, depth, totalMovesCount, frogCount, gridValues);
            EditorUtility.SetDirty(gridData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
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

            LevelDataSO newLevelData = CreateInstance<LevelDataSO>();
            newLevelData.Initialize(width, depth, totalMovesCount, frogCount, gridValues);
            AssetDatabase.CreateAsset(newLevelData, path);
            EditorUtility.SetDirty(newLevelData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newLevelData;
        }
    }
}