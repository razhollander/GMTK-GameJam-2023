using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LandHeatAddedText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private float _animationTime = 1;
    [SerializeField] private int _addedYOffset=1;
    
    public async UniTask Play(int heatAmount)
    {
        gameObject.SetActive(true);
        
        _text.text =  heatAmount > 0? "+" + heatAmount : heatAmount.ToString();
        transform.DOMoveY(transform.position.y + _addedYOffset, _animationTime).OnComplete(OnCompleteEffect).SetEase(Ease.OutCubic);
        _text.DOFade(0, _animationTime).SetEase(Ease.InQuart);
    }

    private void OnCompleteEffect()
    {
        gameObject.SetActive(false);
    }
}
