using UnityEngine;

public class GrapeGridCell : GridCellBase, IInteractableCell, ICollectable
{
    [field: SerializeField] public ContentColor GridColor { get; set; }

    [SerializeField] private Grape grape;
    
    private TextureChanger _textureChanger;

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
    
    public void Interact(ICellInteractable cellInteractable)
    {
        grape.AnimateGrape();
    }
    
    public void DeInteract(ICellInteractable cellInteractable)
    {
        Disappear();
    }


    public void Collect(ICollector collector, Vector3[] path, float duration)
    {
        grape.Collect(collector, path, duration);
    }
}