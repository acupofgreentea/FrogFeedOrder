using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Grape : MonoBehaviour, ISelectable
{
    [SerializeField] private GrapeGridCell _gridCell;
    [SerializeField] private TextureChanger _textureChanger;
    public bool IsSelectable { get; private set; } = true;

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

    public void Collect(ICollector collector, Vector3[] path, float duration)
    {
        transform.DOPath(path, duration, PathType.Linear).SetEase(Ease.Linear)
            .OnComplete(() => { gameObject.SetActive(false); });
        transform.parent = null;
    }
}