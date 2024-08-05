using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Frog : MonoBehaviour, ISelectable, ICellInteractable, ICollector
{
    [field: SerializeField] public ContentColor ContentColor { get; private set; }
    [field: SerializeField] public Direction Direction { get; private set; }
    public bool IsSelectable { get; private set; } = true;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineRendererStartPosition;

    [SerializeField] private GridCellBase currentGridCell;

    private const float tongueMovementDuration = 0.5f;
    private GridCellBase moveStartGridCell;
    private TextureChanger _textureChanger;

    private List<GrapeGridCell> collectedGrapes = new();
    public UnityAction<ICollector> OnSuccess { get; set; }
    public UnityAction<ICollector> OnFail { get; set; }

    private void Awake()
    {
        _textureChanger = GetComponent<TextureChanger>();
        GrapeGridCell.OnGrapeCollected += OnGrapeCollected;
    }

    private void OnDisable()
    {
        GrapeGridCell.OnGrapeCollected -= OnGrapeCollected;
    }

    public void Initialize(ContentColor color, Direction direction)
    {
        ContentColor = color;
        Direction = direction;
        RotateTowardsDirection();
        _textureChanger.ChangeTexture(GameManager.Instance.FrogTextureHolder.GetTextureByColor(color));
    }

    private bool IsSameColor(ContentColor targetColor) => targetColor == ContentColor;

    private void OnGrapeCollected(ICellInteractable cellInteractable, GrapeGridCell grapeGridCell, Grape grape)
    {
        if (cellInteractable.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
            return;

        collectedGrapes.Add(grapeGridCell);
    }


    public void OnSelected(out ICollector collector)
    {
        collector = this;

        moveStartGridCell = currentGridCell;
        var nextCell = currentGridCell.GetTopGridCellInDirection(Direction);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, lineRendererStartPosition.position);
        MoveToNextCell(nextCell, 1);
        IsSelectable = false;
    }


    private void OnFailed()
    {
        MoveBackToFrog(onSequenceFinished: () =>
        {
            lineRenderer.positionCount = 1;
            currentGridCell = moveStartGridCell;
            OnFail?.Invoke(this);
            IsSelectable = true;
        });
    }

    private void HandleOnSuccess()
    {
        MoveBackToFrog(onSequenceFinished: () => { OnSuccess?.Invoke(this); });

        StartCoroutine(Sequence());

        IEnumerator Sequence()
        {
            for (var i = collectedGrapes.Count - 1; i >= 0; i--)
            {
                var grape = collectedGrapes[i];
                grape.MoveGrape(transform.position, (i + 1) * tongueMovementDuration);

                yield return new WaitForSeconds(0.4f);
            }

            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
        }
    }

    private void MoveBackToFrog(UnityAction onSequenceFinished = null)
    {
        int totalCount = lineRenderer.positionCount;
        var lastPosition = lineRenderer.GetPosition(totalCount - 1);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(1, lastPosition);

        float totalTime = tongueMovementDuration * (totalCount - 1);
        float elapsedTime = 0f;
        var targetPosition = lineRenderer.GetPosition(0);
        var startPosition = lastPosition;

        StartCoroutine(Sequence());

        IEnumerator Sequence()
        {
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                float step = Mathf.Min(1, elapsedTime / totalTime);
                lineRenderer.SetPosition(1, Vector3.Lerp(startPosition, targetPosition, step));
                yield return null;
            }

            onSequenceFinished?.Invoke();
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

        lineRenderer.positionCount = index + 1;
        var startPosition = lineRenderer.GetPosition(index - 1);
        var targetPosition = startPosition;
        targetPosition.z = cell.transform.position.z;

        float totalTime = .5f;
        float elapsedTime = 0f;

        StartCoroutine(Sequence());

        IEnumerator Sequence()
        {
            while (elapsedTime <= totalTime)
            {
                elapsedTime += Time.deltaTime;
                lineRenderer.SetPosition(index,
                    Vector3.Lerp(startPosition, targetPosition, elapsedTime / totalTime));
                yield return null;
            }

            HandleOnReachedCell(cell, index);
        }
    }

    private void HandleOnReachedCell(GridCellBase cell, int index)
    {
        currentGridCell = cell;
        if (cell is not IInteractableCell interactable)
        {
            Debug.LogError("wrong move!");
            OnFailed();
            return;
        }

        interactable.Interact(this);
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

    private void RotateTowardsDirection()
    {
        switch (Direction)
        {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
        }
    }
}