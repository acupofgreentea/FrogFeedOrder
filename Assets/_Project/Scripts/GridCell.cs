using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class GridCell : MonoBehaviour, ISelectable
{
    [SerializeField] private SerializedDictionary<Direction, List<GridCell>> _neighbors = new ();
    
    [CanBeNull]
    public GridCell GetTopGridCellInDirection(Direction direction)
    {
        return _neighbors[direction][^1];
    }

    public void OnSelected()
    {
        Disappear();
    }

    private void Disappear()
    {
        transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void SetNeighbors(Direction direction, List<GridCell> neighbors)
    {
        _neighbors[direction] = neighbors;
    }
}