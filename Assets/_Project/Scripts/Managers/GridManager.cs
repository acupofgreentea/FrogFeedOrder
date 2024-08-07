using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<Vector3, List<GridCellBase>> Cells = new();
    
    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        foreach (var gridCellBases in Cells.Values)
        {
            gridCellBases.ForEach(gridCellBase => gridCellBase.OnGridCellDisappear += OnGridCellDisappear);
        }
    }

    public List<GridCellBase> GetCells(Vector3 key)
    {
        bool exists= Cells.TryGetValue(key, out var gridCellBases);

        if (!exists)
            return null;
        
        return gridCellBases;
    }

    public void CopyCells(SerializedDictionary<Vector3, List<GridCellBase>> cells)
    {
        Cells.Clear();
        this.Cells = cells;
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