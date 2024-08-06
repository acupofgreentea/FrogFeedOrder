using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public abstract class GridCellBase : MonoBehaviour
{
    [SerializeField] protected SerializedDictionary<Direction, List<GridCellBase>> _neighbors = new ();
    [field: SerializeField] public GridState State { get; set; }

    public event UnityAction<GridCellBase> OnGridCellDisappear;

    protected virtual void Start()
    {
        foreach (var gridCellBases in _neighbors.Values)
        {
            gridCellBases.ForEach(gridCellBase => gridCellBase.OnGridCellDisappear += OnNeighborGridCellDisappear);
        }
    }
    public abstract void Initialize(params object[] args);

    private void OnNeighborGridCellDisappear(GridCellBase cellBase)
    {
        foreach (var gridCellBases in _neighbors.Values)
        {
            if (gridCellBases.Contains(cellBase))
            {
                gridCellBases.Remove(cellBase);
            }
        }
    }

    [CanBeNull]
    public GridCellBase GetTopGridCellInDirection(Direction direction)
    {
        return _neighbors.TryGetValue(direction, out var neighbors) ? neighbors[^1] : null;
    }

    protected void Disappear()
    {
        transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            OnGridCellDisappear?.Invoke(this);
            gameObject.SetActive(false);
        });
    }

    public void SetNeighbors(Direction direction, List<GridCellBase> neighbors)
    {
        _neighbors[direction] = neighbors;
    }
}