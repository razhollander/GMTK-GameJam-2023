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
    
    public static ShakeCamera Instance;
    private Coroutine _shakeCoroutine;
    private Vector3 _startPos;
    private void Awake()
    {
        Instance = this;
        _startPos = transform.position;
    }

    public void Shake()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }
        
        _shakeCoroutine = StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        yield return transform.DOShakePosition(_duration, _strength, _vibrato, _randomness);
        transform.position = _startPos;
    }
}
