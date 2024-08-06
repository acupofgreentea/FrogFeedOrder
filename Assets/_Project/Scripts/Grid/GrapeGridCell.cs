using DG.Tweening;
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

    protected override void Start()
    {
        grape.gameObject.SetActive(false);
        base.Start();
        _textureChanger.ChangeTexture(GameManager.Instance.SquareTextureHolder.GetTextureByColor(GridColor));
    }

    public override void Initialize(params object[] args)
    {
        GridColor = (ContentColor) args[0];
        State = GridState.Grape;
    }

    protected override void Appear(bool instant)
    {
        base.Appear(instant);
        grape.gameObject.SetActive(true);
        if (instant)
        {
            grape.transform.localScale = Vector3.one;
        }
        else
        {
            grape.transform.localScale = Vector3.zero;
            grape.transform.DOScale(1f, appearDuration);
        }
    }

    public void Interact(ICellInteractable cellInteractable, out bool successfulInteraction)
    {
        successfulInteraction = true;
        grape.AnimateGrape();
        AudioManager.Instance.PlaySound(GameManager.Instance.PopClip);
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