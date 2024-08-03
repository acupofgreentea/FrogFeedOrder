using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelDataSO : ScriptableObject
{
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public List<int> GridValues { get; private set; }

    public void Initialize(int width, int height, int[,] values)
    {
        this.Width = width;
        this.Height = height;
        GridValues = new List<int>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GridValues.Add(values[x, y]);
            }
        }
    }
    public int[,] GetGridValues()
    {
        int[,] values = new int[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                values[x, y] = GridValues[y * Width + x];
            }
        }
        return values;
    }
}