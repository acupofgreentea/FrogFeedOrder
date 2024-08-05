using UnityEngine;
using UnityEngine.Events;

public class GrapeGridCell : GridCellBase, IInteractableCell
{
    [field: SerializeField] public ContentColor GridColor { get; set; }
    [SerializeField] private Grape grape;
    
    private TextureChanger _textureChanger;

    public static event UnityAction<ICellInteractable, GrapeGridCell, Grape> OnGrapeCollected;

    private void Awake()
    {
        _textureChanger = GetComponent<TextureChanger>();
    }

    public override void Initialize(params object[] args)
    {
        GridColor = (ContentColor) args[0];
        State = GridState.Grape;
        _textureChanger.ChangeTexture(GameManager.Instance.FrogTextureHolder.GetTextureByColor(GridColor));
    }

    public void MoveGrape(Vector3 target, float duration)
    {
        grape.MoveToFrog(target, duration);
        Disappear();
    }
    
    public void Interact(ICellInteractable cellInteractable)
    {
        grape.AnimateGrape();
        OnGrapeCollected?.Invoke(cellInteractable, this, grape);
    }

}