using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class GridMover : MonoBehaviour
{
    [SerializeField] private Direction currentDirection = Direction.Up;
    
    [SerializeField] private GridCell _currentCell;

    [Button]
    private void MoveAlongDirection()
    {
        var cellsInDirection = new GridCell[4];
        cellsInDirection[0] = _currentCell;

        for (var i = 1; i < cellsInDirection.Length; i++)
        {
            var cell = cellsInDirection[i - 1].GetTopGridCellInDirection(currentDirection);
            if (cell == null)
            {
                break;
            }
            cellsInDirection[i] = cell;
        }

        transform.DOMove(cellsInDirection[^1].transform.position, 1f);
        _currentCell = cellsInDirection[^1];
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var cell = _currentCell.GetTopGridCellInDirection(currentDirection);
            _currentCell = cell;
            transform.position = _currentCell.transform.position;
        }
    }
    
}