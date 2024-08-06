using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Frog : MonoBehaviour, ICellInteractable, ICollector, ISelectable
{
    [field: SerializeField] public ContentColor ContentColor { get; private set; }
    [field: SerializeField] public Direction Direction { get; set; }
    public bool IsSelectable { get; set; } = true;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineRendererStartPosition;
    [SerializeField] private GridCellBase currentGridCell;
    [SerializeField] private TextureChanger _textureChanger;

    private const float tongueMovementDuration = 0.25f;
    private GridCellBase moveStartGridCell;

    private List<GridCellBase> visitedCells = new();
    public UnityAction<ICollector> OnSuccess { get; set; }
    public UnityAction<ICollector> OnFail { get; set; }

    public static event UnityAction<Frog> OnFrogSpawned;

    private Direction actualDirection;

    public void Initialize(ContentColor color, Direction direction)
    {
        ContentColor = color;
        Direction = direction;
        actualDirection = direction;
        transform.rotation = Helpers.GetRotationByDirection(direction);
        _textureChanger.ChangeTexture(GameManager.Instance.FrogTextureHolder.GetTextureByColor(color));

        OnFrogSpawned?.Invoke(this);
    }

    private bool IsSameColor(ContentColor targetColor) => targetColor == ContentColor;


    public void OnSelected(out ICollector collector)
    {
        collector = this;

        moveStartGridCell = currentGridCell;
        var nextCell = currentGridCell.GetTopGridCellInDirection(Direction);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, lineRendererStartPosition.position);
        visitedCells.Clear();
        visitedCells.Add(currentGridCell);
        MoveToNextCell(nextCell, 1);
        IsSelectable = false;
    }

    private void HandleOnFail()
    {
        MoveToPreviousCell(false);
    }

    private void HandleOnSuccess()
    {
        MoveToPreviousCell(true);
    }

    private void CollectIfExists(bool isSuccess, GridCellBase currentGridCellBase)
    {
        if (!isSuccess)
            return;

        if (currentGridCellBase is not IInteractableCell interactableCell)
            return;

        interactableCell.DeInteract(this);

        if (currentGridCellBase is not ICollectable collectable)
            return;

        GameObject[] moved = new GameObject[visitedCells.Count];
        for (var i = 0; i < visitedCells.Count; i++)
        {
            var movedInteractableCell = visitedCells[i];
            moved[i] = movedInteractableCell.gameObject;
        }

        moved = moved.Reverse().ToArray();
        collectable.Collect(this, Helpers.GetPath(moved),
            (lineRenderer.positionCount - 1) * tongueMovementDuration);
    }

    private void MoveToPreviousCell(bool isSuccess)
    {
        var startPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        var currentInteractableCell = visitedCells[^1];
        var previousInteractableCell = visitedCells.Count > 1 ? visitedCells[^2] : null;

        Vector3 targetPosition = previousInteractableCell == null
            ? moveStartGridCell.transform.position
            : previousInteractableCell.gameObject.transform.position;

        targetPosition.y = startPosition.y;

        CollectIfExists(isSuccess, currentInteractableCell);

        float elapsedTime = 0f;

        HapticManager.LightHaptic();
        StartCoroutine(Sequence());

        IEnumerator Sequence()
        {
            while (elapsedTime <= tongueMovementDuration)
            {
                elapsedTime += Time.deltaTime;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1,
                    Vector3.Lerp(startPosition, targetPosition, elapsedTime / tongueMovementDuration));
                yield return null;
            }

            HandleOnReachedPreviousGridCell(isSuccess);
        }
    }

    private void HandleOnReachedPreviousGridCell(bool isSuccess)
    {
        lineRenderer.positionCount -= 1;

        visitedCells.RemoveAt(visitedCells.Count - 1);

        if (lineRenderer.positionCount > 1)
            MoveToPreviousCell(isSuccess);
        else
        {
            if (isSuccess)
            {
                OnSuccess?.Invoke(this);
                var lastCell = visitedCells[^1];
                if (lastCell is IInteractableCell interactableCell)
                {
                    interactableCell.DeInteract(this);
                }
            }
            else
            {
                lineRenderer.positionCount = 1;
                currentGridCell = moveStartGridCell;
                OnFail?.Invoke(this);
                IsSelectable = true;
                Direction = actualDirection;
            }
        }
    }

    private void MoveToNextCell(GridCellBase cell, int index)
    {
        if (cell == null)
        {
            Debug.LogError("next cell is null so return");
            HandleOnSuccess();
            return;
        }

        if (visitedCells.Contains(cell))
        {
            Debug.LogError("already moved to this cell");
            HandleOnSuccess();
            return;
        }

        HapticManager.LightHaptic();
        lineRenderer.positionCount = index + 1;
        var startPosition = lineRenderer.GetPosition(index - 1);
        var targetPosition = cell.transform.position;
        targetPosition.y = startPosition.y;

        float elapsedTime = 0f;

        StartCoroutine(Sequence());

        IEnumerator Sequence()
        {
            while (elapsedTime <= tongueMovementDuration)
            {
                elapsedTime += Time.deltaTime;
                lineRenderer.SetPosition(index,
                    Vector3.Lerp(startPosition, targetPosition, elapsedTime / tongueMovementDuration));
                yield return null;
            }

            HandleOnReachedNextGridCell(cell, index);
        }
    }


    private void HandleOnReachedNextGridCell(GridCellBase cell, int index)
    {
        currentGridCell = cell;
        visitedCells.Add(cell);
        if (cell is not IInteractableCell interactable) // hit white one so win
        {
            HandleOnSuccess();
            return;
        }

        interactable.Interact(this, out bool successfulInteraction);
        if (!IsSameColor(interactable.GridColor))
        {
            Debug.LogError("not same color");
            HandleOnFail();
        }
        else
        {
            if (!successfulInteraction) //we hit frog or smthng else in the future even though its same color
            {
                HandleOnFail();
                return;
            }
            var nextCell = currentGridCell.GetTopGridCellInDirection(Direction);
            if(CanMoveToNextcell(nextCell))
                MoveToNextCell(nextCell, ++index);
            else
                HandleOnSuccess();
        }
    }

    private bool CanMoveToNextcell(GridCellBase nextCell)
    {
        if(currentGridCell == null || nextCell == null)
            return false;

        if (nextCell.State == GridState.Empty)
            return false;

        if (currentGridCell.State == GridState.Grape && nextCell.State == GridState.Grape)
        {
            bool sameColor = ((GrapeGridCell)currentGridCell).GridColor == ((GrapeGridCell)nextCell).GridColor;

            if (!sameColor)
                return false;
        }

        return true;
    }
}