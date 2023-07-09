using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class SkyManager : MonoBehaviour, IHeatIntervalObserver
{
    public Material skyBoxMaterial;
    public Gradient skyGradient;
    public Gradient lightGradient;
    [Range(0,1)]public float amount;
    private float _nextAmount;
    public Light worldLight;
    [SerializeField] private float transitionsSpeed = 1;
    [SerializeField] private ParticleSystem smoke;
    private static readonly int Top = Shader.PropertyToID("_SkyGradientTop");
    private static readonly int Bottom = Shader.PropertyToID("_SkyGradientBottom");
    public static SkyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HeatSystem.Instance.AddHeatIntervalObserver(this);
        amount = 0;
    }

    private void OnDestroy()
    {
        HeatSystem.Instance.RemoveHeatIntervalObserver(this);
    }

    private void Update()
    {
        if (amount < _nextAmount) amount += Time.deltaTime * transitionsSpeed;
        if (amount > _nextAmount) amount -= Time.deltaTime * transitionsSpeed;
        amount = Mathf.Clamp(amount, 0f, 1f);
        
        skyBoxMaterial.SetColor(Top, skyGradient.Evaluate(amount));
        skyBoxMaterial.SetColor(Bottom, skyGradient.Evaluate(amount));
        worldLight.color = lightGradient.Evaluate(amount);
        worldLight.intensity = 1 - amount;
    }

    public void OnHeatInterval(float newHeat, float deltaHeat)
    {
        if (newHeat / 100f > .5f)
        {
            _nextAmount = math.remap(50, 100, 0f, 1f,newHeat);
            var emission = smoke.emission;
            if (newHeat > 80)
            {
                emission.rateOverTime = newHeat - 10;
            }
            else
            {
                emission.rateOverTime = newHeat - 50;
            }
            
        }

    }

    public void StopSmoke()
    {
        var emission = smoke.emission;
        smoke.Stop();
        emission.rateOverTime = 0;
        
    }
    
}
