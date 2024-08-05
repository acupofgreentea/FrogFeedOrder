using DG.Tweening;
using UnityEngine;

public class FrogGridCell : GridCellBase, IInteractableCell
{
    [field: SerializeField] public ContentColor GridColor { get; set; }
    [field: SerializeField] public Direction Direction { get; private set; }
    [SerializeField] private Frog frog;
    private TextureChanger _textureChanger;

    private void Awake()
    {
        _textureChanger = GetComponent<TextureChanger>();
        frog.OnSuccess += OnFrogSuccess;
    }

    private void OnFrogSuccess(ICollector frog)
    {
        if (this.frog != (Frog)frog)
            return;
        
        Disappear();
    }

    public override void Initialize(params object[] args)
    {
        GridColor = (ContentColor) args[0];
        State = GridState.Frog;
        Direction = (Direction) args[1];
        _textureChanger.ChangeTexture(GameManager.Instance.FrogTextureHolder.GetTextureByColor(GridColor));
        frog.Initialize(GridColor, Direction);
    }

    public void Interact(ICellInteractable cellInteractable)
    {
        transform.DOScale(1.25f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
}