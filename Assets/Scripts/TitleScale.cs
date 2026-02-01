using System;
using DG.Tweening;
using UnityEngine;

public class TitleScale : MonoBehaviour
{
    [SerializeField] private Transform title; 
    [SerializeField] private float scaleAmount = 1.1f; 
    [SerializeField] private float duration = 1f;   

    private void Start()
    {
   
        Vector3 originalScale = title.localScale;

        title.DOScale(originalScale * scaleAmount, duration)
            .SetLoops(-1, LoopType.Yoyo)  
            .SetEase(Ease.InOutSine);    
    }

}
