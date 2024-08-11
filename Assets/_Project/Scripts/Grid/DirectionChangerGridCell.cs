using UnityEngine;

public class DirectionChangerGridCell : GridCellBase, IInteractableCell, IInitializableGridCell
{
    [SerializeField] private Direction newDirection;

    [field: SerializeField] public ContentColor GridColor { get; set; }

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TextureChanger _textureChanger;

#if UNITY_EDITOR
    
    public void Initialize( GridCellData cellData, int index)
    {
        GridColor = cellData.colors[index];
        State = GridState.DirectionChanger;
        newDirection = cellData.directions[index];
        
        _textureChanger.ChangeMaterial(Helpers.FindObjectByName<MaterialHolderSO>("SquareMaterialHolder").GetMaterialByColor(GridColor));
    }
#endif
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