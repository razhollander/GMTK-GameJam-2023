using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _strength = 1f;
    [SerializeField] private int _vibrato = 10;
    [SerializeField] private float _randomness = 10f;
    
    [SerializeField] private float _durationSmall = 0.25f;
    [SerializeField] private float _strengthSmall = 0.25f;
    [SerializeField] private int _vibratoSmall = 10;
    [SerializeField] private float _randomnessSmall = 10f;
    public static ShakeCamera Instance;
    private Coroutine _shakeCoroutine;
    private Vector3 _startPos;
    private void Awake()
    {
        Instance = this;
        _startPos = transform.position;
    }

    public void Shake(bool isSmall = false)
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            transform.position = _startPos;
        }
        
        _shakeCoroutine = StartCoroutine(isSmall ? DoShakeSmall() : DoShake());
    }

    private IEnumerator DoShake()
    {
        yield return transform.DOShakePosition(_duration, _strength, _vibrato, _randomness);
        transform.position = _startPos;
    }
    
    private IEnumerator DoShakeSmall()
    {
        yield return transform.DOShakePosition(_durationSmall, _strengthSmall, _vibratoSmall, _randomnessSmall);
        transform.position = _startPos;
    }
}
