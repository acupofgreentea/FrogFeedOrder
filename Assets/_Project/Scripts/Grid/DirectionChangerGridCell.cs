using UnityEngine;

public class DirectionChangerGridCell : GridCellBase, IInteractableCell
{
    [SerializeField] private Direction newDirection;

    [field: SerializeField] public ContentColor GridColor { get; set; }

    public override void Initialize(params object[] args)
    {
        GridColor = (ContentColor) args[0];
        State = GridState.DirectionChanger;
        newDirection = (Direction) args[1];
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