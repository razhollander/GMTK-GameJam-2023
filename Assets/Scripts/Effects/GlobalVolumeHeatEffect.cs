using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class GlobalVolumeHeatEffect : MonoBehaviour, IHeatIntervalObserver
{
    [SerializeField] private Volume _volume;
    [SerializeField] private float _bloomIntensity = 1f;
    
    void Start()
    {
        UpdateEffectIntensity(HeatSystem.Instance.CurrentHeat);
    }

    public void OnHeatInterval(float newHeat, float deltaHeat)
    {
        UpdateEffectIntensity(newHeat);
    }

    [ContextMenu("Test")]
    public void Test()
    {
        UpdateEffectIntensity(50);
    }
    
    private void UpdateEffectIntensity(float newHeat)
    {
        Bloom bloom;
        _volume.profile.TryGet<Bloom>(out bloom);
        bloom.intensity.value = newHeat * 0.01f * _bloomIntensity;
    }
}
