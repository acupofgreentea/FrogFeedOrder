using DG.Tweening;
using UnityEngine;

public class Grape : MonoBehaviour, ISelectable
{
    [SerializeField] private GrapeGridCell _gridCell;
    public bool IsSelectable { get; private set; } = true;

    private TextureChanger _textureChanger;

    private void Awake()
    {
        _textureChanger = GetComponent<TextureChanger>();
    }

    private void Start()
    {
        _textureChanger.ChangeTexture(GameManager.Instance.GrapeTextureHolder.GetTextureByColor(_gridCell.GridColor));
    }

    public void AnimateGrape()
    {
        transform.DOScale(1.25f, 0.25f).SetLoops(2, LoopType.Yoyo).OnComplete(() => IsSelectable = true);
    }

    public void MoveToFrog(Vector3 target, float duration)
    {
        transform.DOMove(target, duration).SetEase(Ease.Linear).OnComplete(
            () => { gameObject.SetActive(false); });

        transform.parent = null;
    }

    public void OnSelected(out ICollector collector)
    {
        IsSelectable = false;
        collector = null;
        AnimateGrape();
    }
}