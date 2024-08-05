using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Frog : MonoBehaviour, ICellInteractable, ICollector, ISelectable
{
    [field: SerializeField] public ContentColor ContentColor { get; private set; }
    [field: SerializeField] public Direction Direction { get; set; }
    public bool IsSelectable { get; private set; } = true;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineRendererStartPosition;
    [SerializeField] private GridCellBase currentGridCell;
    [SerializeField] private TextureChanger _textureChanger;

    private const float tongueMovementDuration = 0.25f;
    private GridCellBase moveStartGridCell;

    private List<IInteractableCell> movedInteractableCells = new();
    public UnityAction<ICollector> OnSuccess { get; set; }
    public UnityAction<ICollector> OnFail { get; set; }

    private Direction actualDirection;

    public void Initialize(ContentColor color, Direction direction)
    {
        ContentColor = color;
        Direction = direction;
        actualDirection = direction;
        transform.rotation = Helpers.GetRotationByDirection(direction);
        _textureChanger.ChangeTexture(GameManager.Instance.FrogTextureHolder.GetTextureByColor(color));
    }

    private bool IsSameColor(ContentColor targetColor) => targetColor == ContentColor;


    public void OnSelected(out ICollector collector)
    {
        collector = this;

        moveStartGridCell = currentGridCell;
        var nextCell = currentGridCell.GetTopGridCellInDirection(Direction);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, lineRendererStartPosition.position);
        movedInteractableCells.Clear();
        movedInteractableCells.Add(currentGridCell as IInteractableCell);
        MoveToNextCell(nextCell, 1);
        IsSelectable = false;
    }

    private void OnFailed()
    {
        MoveToPreviousCell(false);
    }

    private void HandleOnSuccess()
    {
        MoveToPreviousCell(true);
    }

    private void CollectIfExists(bool isSuccess, IInteractableCell currentInteractableCell)
    {
        if (!isSuccess)
            return;
        currentInteractableCell.DeInteract(this);
        if (currentInteractableCell is ICollectable collectable)
        {
            GameObject[] moved = new GameObject[movedInteractableCells.Count];
            for (var i = 0; i < movedInteractableCells.Count; i++)
            {
                var movedInteractableCell = movedInteractableCells[i];
                moved[i] = movedInteractableCell.gameObject;
            }

            moved = moved.Reverse().ToArray();
            collectable.Collect(this, Helpers.GetPath(moved),
                (lineRenderer.positionCount - 1) * tongueMovementDuration);
        }
    }

    private void MoveToPreviousCell(bool isSuccess)
    {
        var startPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        var currentInteractableCell = movedInteractableCells[^1];
        var previousInteractableCell = movedInteractableCells.Count > 1 ? movedInteractableCells[^2] : null;
        var targetPosition = previousInteractableCell.gameObject.transform.position;
        targetPosition.y = startPosition.y;

        CollectIfExists(isSuccess, currentInteractableCell);

        float elapsedTime = 0f;

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

            HandleOnReachedPreviousGridCell(isSuccess, currentInteractableCell);
        }
    }

    private void HandleOnReachedPreviousGridCell(bool isSuccess, IInteractableCell currentInteractableCell)
    {
        lineRenderer.positionCount -= 1;

        movedInteractableCells.RemoveAt(movedInteractableCells.Count - 1);

        if (lineRenderer.positionCount > 1)
            MoveToPreviousCell(isSuccess);
        else
        {
            if (isSuccess)
            {
                StartCoroutine(Delay());

                IEnumerator Delay()
                {
                    yield return new WaitForSeconds(0.25f);
                    OnSuccess?.Invoke(this);
                    movedInteractableCells[^1].DeInteract(this);
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

        if (cell is IInteractableCell interactableCell)
        {
            if (movedInteractableCells.Contains(interactableCell))
            {
                Debug.LogError("already moved to this cell");
                HandleOnSuccess();
                return;
            }
        }

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
        if (cell is not IInteractableCell interactable)
        {
            Debug.LogError("wrong move!");
            OnFailed();
            return;
        }

        interactable.Interact(this);
        movedInteractableCells.Add(interactable);
        if (!IsSameColor(interactable.GridColor))
        {
            Debug.LogError("not same color");
            OnFailed();
        }
        else
        {
            MoveToNextCell(currentGridCell.GetTopGridCellInDirection(Direction), ++index);
        }
    }
}