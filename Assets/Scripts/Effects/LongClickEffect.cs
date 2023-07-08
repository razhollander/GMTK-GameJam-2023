using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LongClickEffect : MonoBehaviour
{
    [SerializeField] private Image _barImage;

    [SerializeField] private Vector2 _offset = new Vector2(1,1);
    [SerializeField] private Ease _ease = Ease.Linear;
    private CancellationTokenSource disableCancellationToken;

    public static LongClickEffect Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async UniTask StartFillBar(float duration, CancellationTokenSource cs)
    {
        _barImage.fillAmount = 0;

        await DOTween.To(() => _barImage.fillAmount, (x) =>
        {
            _barImage.fillAmount = x;
            transform.position = Input.mousePosition + _offset.ToVector3XY();
        }, 1, duration).SetEase(_ease).ToUniTask(cancellationToken: cs.Token);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
