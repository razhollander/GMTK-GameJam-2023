using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HeatMeter : MonoBehaviour, IHeatIntervalObserver
{
    [SerializeField] private TextMeshProUGUI _deltaHeatText;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _heatText;
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private Ease _animationEase = Ease.OutCubic;
    [SerializeField] private float deltaTextAnimationDuration = 1f;
    [SerializeField] private float deltaTextMoveY = 50f;
    [SerializeField] private Color _deltaHeatTextColorNegative;
    [SerializeField] private Color _deltaHeatTextColorPositive;
    private RectTransform _rectTransform;
    private Vector3 _deltaTextStartPos;
    private void Awake()
    {
        _slider.onValueChanged.AddListener(UpdateHeatText);
        _rectTransform = GetComponent<RectTransform>();
        _deltaTextStartPos = _deltaHeatText.transform.localPosition;
    }

    void Start()
    {
        HeatSystem.Instance.AddHeatIntervalObserver(this);
        UpdateSliderByHeat(HeatSystem.Instance.CurrentHeat);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(UpdateHeatText);
        HeatSystem.Instance.RemoveHeatIntervalObserver(this);
    }

    private void UpdateSliderByHeat(float heat)
    {
        _slider.DOValue(heat / 100f, _animationDuration).SetEase(_animationEase);
    }
    public void OnHeatInterval(float newHeat, float deltaHeat)
    {
        UpdateSliderByHeat(newHeat);
        ShowDeltaHeatText(deltaHeat);
    }

    private void ShowDeltaHeatText(float deltaHeat)
    {
        _deltaHeatText.text = deltaHeat > 0 ? "+"+deltaHeat.ToString() + "°" : deltaHeat.ToString() + "°";
        _deltaHeatText.color = deltaHeat > 0 ? _deltaHeatTextColorNegative : _deltaHeatTextColorPositive;
        _deltaHeatText.DOFade(0, deltaTextAnimationDuration).SetEase(Ease.InCubic);
        var transform1 = _deltaHeatText.transform;
        transform1.localPosition = _deltaTextStartPos;
        transform1.DOLocalMoveX(transform1.localPosition.y + deltaTextMoveY, deltaTextAnimationDuration).SetEase(Ease.OutCubic);
    }

    private void UpdateHeatText(float val)
    {
        var numberstring = $"{val * 100f:F1}";

        if (numberstring[^1] == '0')
        {
            numberstring = $"{val * 100f:F0}";
        }
        _heatText.text = numberstring + "°";
    }
}
