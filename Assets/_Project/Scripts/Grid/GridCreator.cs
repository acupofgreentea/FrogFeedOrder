using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private float yOffset = 0.05f;

    [SerializeField] private SerializedDictionary<Vector3, List<GridCellBase>> Cells = new();

    private void Awake()
    {
        LevelManager.OnLevelLoaded += CreateGrid;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= CreateGrid;
    }

    private void CreateGrid(LevelDataSO levelDataSO)
    {
        CreateGridFromData(levelDataSO);
    }

    private void CreateGridFromData(LevelDataSO levelDataSO)
    {
        if (levelDataSO == null || levelDataSO.gridCells == null)
        {
            Debug.LogWarning("GridData or gridCells is not assigned or initialized.");
            return;
        }

        ClearExistingGrid();

        var originPosition = new Vector3(-(levelDataSO.Width - 1) / 2f, 0f, -(levelDataSO.Depth - 1) / 2f);

        GridCellData[,] values = levelDataSO.GetGridValues();

        var prefabHolder = GameManager.Instance.GridCellPrefabHolder;

        for (int x = 0; x < levelDataSO.Width; x++)
        {
            for (int z = 0; z < levelDataSO.Depth; z++)
            {
                var gridCellData = values[x, z];
                for (int y = 0; y < gridCellData.height; y++)
                {
                    Vector3 spawnPosition = new Vector3(x, y * yOffset, (levelDataSO.Depth - 1 - z)) + originPosition;
                    Vector3 key = new Vector3(spawnPosition.x, 0f, spawnPosition.z);
                    var spawned = Instantiate(prefabHolder.GetPrefabByType(gridCellData.states[y]), spawnPosition,
                        Quaternion.identity, transform);
                    if (!Cells.ContainsKey(key))
                        Cells.Add(key, new List<GridCellBase>());

                    spawned.OnGridCellDisappear += OnGridCellDisappear;

                    Cells[key].Add(spawned);
                    InitializeGridCell(spawned, gridCellData, y);
                }
            }
        }

        PopulateNeighbors();
    }

    private void InitializeGridCell(GridCellBase cell, GridCellData cellData, int index)
    {
        //0-color 1-direction
        if (cellData.colors.Length > 0 && cellData.directions.Length > 0) //if has color and direction -> froggridcell
            cell.Initialize(cellData.colors[index], cellData.directions[index]);
        else if (cellData.directions.Length == 0) //if has no direction -> grapegridcell
            cell.Initialize(cellData.colors[index]);
        else if(cellData.states.Any(x => x == GridState.DirectionChanger))
            cell.Initialize(cellData.colors[index], cellData.directions[index]);
        else //empty cell
            cell.Initialize();
    }


    private void PopulateNeighbors()
    {
        foreach (var cell in Cells)
        {
            foreach (var direction in (Direction[])System.Enum.GetValues(typeof(Direction)))
            {
                var neighbors = new List<GridCellBase>();
                var directionVector = Helpers.GetDirectionVector(direction);
                var key = cell.Key + directionVector;
                if (Cells.TryGetValue(key, out var value))
                {
                    neighbors.AddRange(value);
                    foreach (var gridCell in cell.Value)
                    {
                        gridCell.SetNeighbors(direction, neighbors);
                    }
                }
            }
        }
    }

    [Button]
    private void ClearExistingGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Cells.Clear();
    }

    private void OnGridCellDisappear(GridCellBase gridCellBase)
    {
        foreach (var cell in Cells)
        {
            if (cell.Value.Contains(gridCellBase))
            {
                cell.Value.Remove(gridCellBase);
                break;
            }
        }
    }
}