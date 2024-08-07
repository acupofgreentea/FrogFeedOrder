using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Grape : MonoBehaviour, ISelectable
{
    [SerializeField] private GrapeGridCell _gridCell;
    [SerializeField] private TextureChanger _textureChanger;
    public bool IsSelectable { get; private set; } = true;

    private const float animateDuration = 0.1f;

    private void Start()
    {
        _textureChanger.ChangeTexture(GameManager.Instance.GrapeTextureHolder.GetTextureByColor(_gridCell.GridColor));
    }

    public void AnimateGrape(UnityAction onComplete = null)
    {
        transform.DOScale(1.25f, animateDuration).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            onComplete?.Invoke();
            IsSelectable = true;
        });
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

    public void FalseAnimateGrape()
    {
        _textureChanger.ChangeTexture(GameManager.Instance.GrapeTextureHolder.GetTextureByColor(ContentColor.Red));
        AnimateGrape(onComplete: () =>
        {
            _textureChanger.ChangeTexture(
                GameManager.Instance.GrapeTextureHolder.GetTextureByColor(_gridCell.GridColor));
        });
    }
}