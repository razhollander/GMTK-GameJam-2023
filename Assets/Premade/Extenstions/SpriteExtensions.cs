using DG.Tweening;
using UnityEngine;

namespace CoreDomain.Scripts.Extensions
{
    public static class SpriteExtensions
    {
        public static void DOColor(this SpriteRenderer spriteRenderer, Color targetColor, float duration, Ease ease = Ease.Linear)
        {
            DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, targetColor, duration).Play().SetEase(ease);
        }
    }
}
