using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootCollectEffect : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private float _animationTime = 1;
    [SerializeField] private int _addedYEffet=1;
    
    public async UniTask Play(int moneyAmount)
    {
        gameObject.SetActive(true);
        _text.text = "+" + moneyAmount;
        transform.DOMoveY(transform.position.y + _addedYEffet, _animationTime).OnComplete(OnCompleteEffect).SetEase(Ease.OutCubic);
        _text.DOFade(0, _animationTime).SetEase(Ease.InQuart);
    }

    private void OnCompleteEffect()
    {
        Destroy(gameObject);
    }
}
