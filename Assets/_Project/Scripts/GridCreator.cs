using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GameObject _gridPrefab;
    [SerializeField] private LevelDataSO levelDataSo;
    [SerializeField] private float yOffset = 0.1f;

    [SerializeField] private SerializedDictionary<Vector3, List<GridCell>> Cells = new();

    private void Start()
    {
        CreateGridFromData();
    }

    public void CreateGridFromData()
    {
        if (levelDataSo == null || levelDataSo.GridValues == null)
        {
            Debug.LogWarning("LevelDataSO or gridValues is not assigned or initialized.");
            return;
        }

        ClearExistingGrid();

        var originPosition = new Vector3(-(levelDataSo.Width - 1) / 2f, 0f, -(levelDataSo.Height - 1) / 2f);

        int[,] values = levelDataSo.GetGridValues(); 

        for (int x = 0; x < levelDataSo.Width; x++)
        {
            for (int z = 0; z < levelDataSo.Height; z++)
            {
                for (int y = 0; y < values[x, z]; y++)
                {
                    Vector3 spawnPosition = new Vector3(x, y * yOffset, z) + originPosition;
                    Vector3 key = new Vector3(spawnPosition.x, 0, spawnPosition.z);
                    var spawned = Instantiate(_gridPrefab, spawnPosition, Quaternion.identity, transform);
                    if (!Cells.ContainsKey(key))
                        Cells.Add(key, new List<GridCell>());
                    
                    Cells[key].Add(spawned.GetComponent<GridCell>());
                }
            }
        }
        
        
        PopulateNeighbors();
    }

    private Vector3 GetDirectionVector(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector3.forward,
            Direction.Down => Vector3.back,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            _ => Vector3.zero
        };
    }

    [Button]
    private void PopulateNeighbors()
    {
        foreach (var cell in Cells)
        {
            foreach (var direction in (Direction[]) System.Enum.GetValues(typeof(Direction)))
            {
                var neighbors = new List<GridCell>();
                var directionVector = GetDirectionVector(direction);
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

    private void ClearExistingGrid()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        Cells.Clear();
    }
}