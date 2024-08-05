using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewGridData", menuName = "Grid/GridData")]
public class LevelDataSO : ScriptableObject
{
    [field: SerializeField] public int Width { get; private set; } = 4;
    [field: SerializeField] public int Depth { get; private set; } = 4;
    [field: SerializeField] public int MoveCount { get; private set; }
    
    [field: SerializeField, ReadOnly] public int FrogCount { get; private set; }
    public List<GridCellData> gridCells;

    public void Initialize(int width, int depth, int moveCount, int frogCount, GridCellData[,] values)
    {
        this.Width = width;
        this.Depth = depth;
        this.FrogCount = frogCount;
        this.MoveCount = moveCount;
        gridCells = new List<GridCellData>();

        for (int y = 0; y < depth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                gridCells.Add(values[x, y]);
            }
        }
    }

    public GridCellData[,] GetGridValues()
    {
        GridCellData[,] values = new GridCellData[Width, Depth];
        for (int y = 0; y < Depth; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                values[x, y] = gridCells[y * Width + x];
            }
        }
        return values;
    }
}