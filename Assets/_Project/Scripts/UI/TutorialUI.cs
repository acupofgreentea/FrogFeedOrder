using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Image handImage;
    
    [SerializeField] private TMP_Text tutorialText;

    private Camera mainCam;
    private Canvas canvas;
    
    public static UnityAction<bool, Vector3, string> UpdateTutorialUI;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        mainCam = Camera.main;
        UpdateTutorialUI += Init;
    }

    private void OnDisable()
    {
        UpdateTutorialUI -= Init;
    }

    private void Init(bool enable, Vector3 worldPosition, string text)
    {
        canvas.enabled = enable;
        handImage.transform.localScale = Vector3.one;
        if (enable == false)
        {
            handImage.transform.DOKill();
            return;
        }
        
        Vector3 screenPosition = mainCam.WorldToScreenPoint(worldPosition);
        panel.transform.position = screenPosition;
        tutorialText.text = text;
        handImage.transform.DOScale(1.25f, 0.35f).SetLoops(-1, LoopType.Yoyo);
    }

}
