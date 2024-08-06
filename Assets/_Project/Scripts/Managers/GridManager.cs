using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<Vector3, List<GridCellBase>> Cells = new();

    private void Awake()
    {
        foreach (var gridCellBases in Cells.Values)
        {
            gridCellBases.ForEach(gridCellBase => gridCellBase.OnGridCellDisappear += OnGridCellDisappear);
        }
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