using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class HeatMeter : MonoBehaviour, IHeatIntervalObserver
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _heatText;
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private Ease _animationEase = Ease.OutCubic;
    private void Awake()
    {
        _slider.onValueChanged.AddListener(UpdateHeatText);
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

    private void UpdateSliderByHeat(int heat)
    {
        _slider.DOValue(heat / 100f, _animationDuration).SetEase(_animationEase);
    }
    public void OnHeatInterval(int newHeat, int deltaHeat)
    {
        UpdateSliderByHeat(newHeat);
    }

    private void UpdateHeatText(float val)
    {
        _heatText.text = ((int)(val*100)).ToString() + "°";
    }
}
