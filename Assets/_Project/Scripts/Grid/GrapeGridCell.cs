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
        Vector3 key = transform.position;
        key.y = 0f;

        var cells = GridManager.Instance.GetCells(key);
        bool existsSameColor = false;

        if (cells != null)
        {
            foreach (var gridCellBase in cells)
            {
                if (gridCellBase == this)
                    continue;
                if(gridCellBase.State != GridState.Grape)
                    continue;

                //if any under cells is the same color as frog that means we fail 
                existsSameColor = ((GrapeGridCell)gridCellBase).GridColor == cellInteractable.ContentColor;

                if (existsSameColor == true)
                    break;
            }
        }
        
        successfulInteraction = !existsSameColor && cellInteractable.ContentColor == GridColor;

        if (successfulInteraction == false)
        {
            AnimeFalseInteraction();
        }
        else
        {
            grape.AnimateGrape();
        }
        AudioManager.Instance.PlaySound(GameManager.Instance.PopClip);
    }

    private void AnimeFalseInteraction()
    {
        grape.FalseAnimateGrape();
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