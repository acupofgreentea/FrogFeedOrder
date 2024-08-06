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

    protected const float appearDuration = 0.35f;
    protected const float disappearDuration = 0.35f;

    protected virtual void Start()
    {
        foreach (var gridCellBases in _neighbors.Values)
        {
            gridCellBases.ForEach(gridCellBase => gridCellBase.OnGridCellDisappear += OnNeighborGridCellDisappear);
        }

        if (!_neighbors.ContainsKey(Direction.Above))
        {
            Appear(true);
        }
    }
    public abstract void Initialize(params object[] args);

    private void OnNeighborGridCellDisappear(GridCellBase cellBase)
    {
        foreach (var gridCellBases in _neighbors)
        {
            if (gridCellBases.Value.Contains(cellBase))
            {
                gridCellBases.Value.Remove(cellBase);

                if (gridCellBases.Key != Direction.Above)
                    break;
                
                Appear(false);
            }
        }
    }

    [CanBeNull]
    public GridCellBase GetTopGridCellInDirection(Direction direction)
    {
        return _neighbors.TryGetValue(direction, out var neighbors) ? neighbors[^1] : null;
    }

    protected virtual void Appear(bool instant)
    {
        
    }

    protected void Disappear()
    {
        transform.DOScale(0f, disappearDuration).OnComplete(() =>
        {
            OnGridCellDisappear?.Invoke(this);
            gameObject.SetActive(false);
        });
    }

    public void SetNeighbors(Direction direction, List<GridCellBase> neighbors)
    {
        _neighbors[direction] = neighbors;
    }
    
    public void AddNeighbors(Direction direction, GridCellBase neighbor)
    {
        if (!_neighbors.TryGetValue(direction, out var neighbors))
        {
            neighbors = new List<GridCellBase>();
            _neighbors[direction] = neighbors;
        }
        neighbors.Add(neighbor);
    }
}