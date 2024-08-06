using DG.Tweening;
using UnityEngine;

public class FrogGridCell : GridCellBase, IInteractableCell
{
    [field: SerializeField] public ContentColor GridColor { get; set; }
    [field: SerializeField] public Direction Direction { get; private set; }
    [SerializeField] private Frog frog;
    private TextureChanger _textureChanger;

    private const float animateDuration = 0.15f;
    private void Awake()
    {
        _textureChanger = GetComponent<TextureChanger>();
        frog.OnSuccess += OnFrogSuccess;
    }

    protected override void Start()
    {
        frog.gameObject.SetActive(false);
        base.Start();
        _textureChanger.ChangeTexture(GameManager.Instance.FrogTextureHolder.GetTextureByColor(GridColor));
        frog.Initialize(GridColor, Direction);
    }

    protected override void Appear(bool instant)
    {
        base.Appear(instant);
        frog.gameObject.SetActive(true);
        if (instant)
        {
            frog.transform.localScale = Vector3.one;
        }
        else
        {
            frog.transform.localScale = Vector3.zero;
            frog.transform.DOScale(1f, appearDuration);
        }
    }

    private void OnFrogSuccess(ICollector frog)
    {
        if (this.frog != (Frog)frog)
            return;
        
        Disappear();
    }
    
    public void DeInteract(ICellInteractable cellInteractable)
    {
        Disappear();
    }

    public override void Initialize(params object[] args)
    {
        GridColor = (ContentColor) args[0];
        State = GridState.Frog;
        Direction = (Direction) args[1];
    }

    public void Interact(ICellInteractable cellInteractable, out bool successfulInteraction)
    {
        successfulInteraction = false; // if we hit a frog we must fail even if it is the same color
        HapticManager.MediumHaptic();
        frog.transform.DOScale(1.25f, animateDuration).SetLoops(2, LoopType.Yoyo);
    }
}