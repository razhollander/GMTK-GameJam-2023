using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatMeter : MonoBehaviour, IHeatIntervalObserver
{
    [SerializeField] private Slider _slider;

    void Start()
    {
        HeatSystem.Instance.AddHeatIntervalObserver(this);
        UpdateSliderByHeat(HeatSystem.Instance.CurrentHeat);
    }

    private void UpdateSliderByHeat(int heat)
    {
        _slider.value = heat / 100f;
    }
    public void OnHeatInterval(int newHeat, int deltaHeat)
    {
        UpdateSliderByHeat(newHeat);
    }
}
