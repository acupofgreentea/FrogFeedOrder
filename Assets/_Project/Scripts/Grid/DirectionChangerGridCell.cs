using UnityEngine;

public class DirectionChangerGridCell : GridCellBase, IInteractableCell
{
    [SerializeField] private Direction newDirection;

    [field: SerializeField] public ContentColor GridColor { get; set; }

    [SerializeField] private SpriteRenderer spriteRenderer;

    public override void Initialize(params object[] args)
    {
        GridColor = (ContentColor)args[0];
        State = GridState.DirectionChanger;
        newDirection = (Direction)args[1];
    }

    protected override void Start()
    {
        base.Start();
        SetRotation();
    }

    private void SetRotation()
    {
        switch (newDirection)
        {
            case Direction.Down:
                break;
            case Direction.Left:
                spriteRenderer.transform.Rotate(Vector3.forward, -90);
                break;
            case Direction.Right:
                spriteRenderer.transform.Rotate(Vector3.forward, 90);
                break;
            case Direction.Up:
                spriteRenderer.transform.Rotate(Vector3.forward, 180);
                break;
        }
    }

    public void DeInteract(ICellInteractable cellInteractable)
    {
        Disappear();
    }


    public void Interact(ICellInteractable cellInteractable, out bool successfulInteraction)
    {
        successfulInteraction = true;
        cellInteractable.Direction = newDirection;
    }
}