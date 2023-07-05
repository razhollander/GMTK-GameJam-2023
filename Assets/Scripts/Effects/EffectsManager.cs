using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsManager : GaneEventListener
{
    [SerializeField] private ParticleSystem _snowEffect;
    [SerializeField] private VolumeProfile VolumeProfile;
    [SerializeField] private Vector2 _alarmAnimationBounds = new Vector2(0.224f, 0.336f);
    [SerializeField] private float _alarmAnimationTime = 2;
    [SerializeField] private GameObject _water;
    [SerializeField] private GameObject _playerBubble;
    [SerializeField] private ParticleSystem _lightingFlashEffect;
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Light2D _playerLight;
    [SerializeField] private float _globalLightOffIntensity = 0;
    
    private Tween alarmAnimationTween;
    private float _globalLightNormalIntensity;
    private Coroutine lightCoroutine;
    private void Awake()
    {
        _globalLightNormalIntensity = _globalLight.intensity;
    }

    public override void OnGameEvent(GameEvent gameEvent)
    {
        StopAll();
        
        switch (gameEvent)
        {
            case GameEvent.Freeze: 
                _snowEffect.gameObject.SetActive(true);
                _snowEffect.Play(); break;
            case GameEvent.Alarm: 
                 
                Vignette vignette;
 
                if(!VolumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
                vignette.active = true;
                vignette.intensity.Override(_alarmAnimationBounds.x);
                alarmAnimationTween = DOTween.To(() => vignette.intensity.value, value => vignette.intensity.Override(value), _alarmAnimationBounds.y, _alarmAnimationTime)
                    .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuart); break;
            
            case GameEvent.Flood:
                _water.SetActive(true);
                _playerBubble.SetActive(true);
                break;
            case GameEvent.Electricity:
                _lightingFlashEffect.gameObject.SetActive(true);
                _lightingFlashEffect.Play();
                lightCoroutine = StartCoroutine(SetLightsInDelay());
                break;
                default: break;
        }
    }

    private IEnumerator SetLightsInDelay()
    {
        yield return new WaitForSeconds(_lightingFlashEffect.main.duration);
        _globalLight.intensity = _globalLightOffIntensity;
        _playerLight.gameObject.SetActive(true);
    }
    [ContextMenu("fire alarm effect")]
    public void FireAlarm()
    {
        OnGameEvent(GameEvent.Alarm);
    }
    
    [ContextMenu("fire freeze effect")]
    public void FireFreeze()
    {
        OnGameEvent(GameEvent.Freeze);
    }
    
    [ContextMenu("fire Flood effect")]
    public void FireWater()
    {
        OnGameEvent(GameEvent.Flood);
    }
    
        
    [ContextMenu("fire electricity effect")]
    public void FireElectricity()
    {
        OnGameEvent(GameEvent.Electricity);
    }
    private void StopAll()
    {
        _snowEffect.gameObject.SetActive(false);
        _snowEffect.Stop();
        
        alarmAnimationTween.Kill();
        
        Vignette vignette;
        if(!VolumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        vignette.active = false;
        
        _water.SetActive(false);
        _playerBubble.SetActive(false);

        if (lightCoroutine != null)
        {
            StopCoroutine(lightCoroutine);
        }
        _lightingFlashEffect.gameObject.SetActive(false);
        _globalLight.intensity = _globalLightNormalIntensity;
        _playerLight.gameObject.SetActive(false);
    }
}
