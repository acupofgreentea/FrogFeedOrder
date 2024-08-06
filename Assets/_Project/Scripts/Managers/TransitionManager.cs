using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetAlpha(0f);
    }

    public void Fade(float totalDuration, UnityAction onFadeInComplete = null,  UnityAction onFadeOutComplete = null)
    {
        FadeIn(totalDuration / 2f, () =>
        {
            onFadeInComplete?.Invoke();
            FadeOut(totalDuration / 2f, ()=> onFadeOutComplete?.Invoke());
        });
    }

    public void FadeIn(float duration, UnityAction onComplete = null)
    {
        SetAlpha(0f);
        canvasGroup.DOFade(1f, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetAlpha(1f);
            onComplete?.Invoke();
        });
    }

    public void FadeOut(float duration, UnityAction onComplete = null)
    {
        SetAlpha(1f);
        canvasGroup.DOFade(0f, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetAlpha(0f);
            onComplete?.Invoke();
        });
    }

    private void SetAlpha(float target)
    {
        canvasGroup.alpha = target;
        canvasGroup.blocksRaycasts = Mathf.Approximately(1f, target);
    }
}