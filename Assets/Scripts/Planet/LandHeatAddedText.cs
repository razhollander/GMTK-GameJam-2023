using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LandHeatAddedText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private float _animationTime = 1;
    [SerializeField] private float _addedYOffset=1f;
    [SerializeField] private float _startingYOffset=1;
    [SerializeField] private Color _colorPositive = Color.green;
    [SerializeField] private Color _colorNegative = Color.red;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private float _minimumScale = 0.5f;
    [SerializeField] private float _scaleStep =0.5f;
    [SerializeField] [Range(0f,1f)] private float _notFullOpacity = 0.5f;
    private Material _material;

    // private void Awake()
    // {
    //     _material = _renderer.material;
    //     _text.material = _material;
    // }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    
    public async UniTask Play(int heatAmount, bool isFullOpacity)
    {
        gameObject.SetActive(true);
        _text.transform.localScale = Vector3.one * (_minimumScale + _scaleStep * Math.Abs(heatAmount));
        var targetOpacity = new Color(1, 1, 1, isFullOpacity ? 1 : _notFullOpacity);
        //_text.outlineColor = isFullOpacity? Color.white : Color.black;
        
        //_text.transform.LookAt(Camera.main.transform);
        //var zAngleRotation = (Quaternion.FromToRotation(transform.up, _text.transform.position) * transform.rotation).eulerAngles.z;
        //_text.transform.rotation = Quaternion.Euler(0, 0, zAngleRotation);
        RotateTextToCamera();
        var screenUpVectorInWorldSpace = Camera.main.transform.up.normalized;
        _text.text =  heatAmount > 0? "+"+heatAmount.ToString()+"°" : heatAmount.ToString()+"°";
        _text.color = (heatAmount > 0 ? _colorNegative : _colorPositive)*targetOpacity;
        _text.transform.localPosition = Vector3.zero;// screenUpVectorInWorldSpace * _startingYOffset;
        _text.transform.DOMove(_text.transform.position + screenUpVectorInWorldSpace*_addedYOffset, _animationTime).OnComplete(OnCompleteEffect).SetEase(Ease.OutCubic);
        _text.DOFade(0, _animationTime).SetEase(Ease.InQuart);
    }

    private void RotateTextToCamera()
    {
        _text.transform.rotation = Camera.main.transform.rotation;
        Vector3 objectNormal = _text.transform.rotation * Vector3.forward;
        Vector3 cameraToText = _text.transform.position - Camera.main.transform.position;
        float f = Vector3.Dot (objectNormal, cameraToText);
        if (f < 0f) 
        {
            _text.transform.Rotate (0f, 180f, 0f);
        }
        
        //_text.transform.rotation = Quaternion.LookRotation((_text.transform.position - Camera.main.transform.position).normalized);
    }

    private void OnCompleteEffect()
    {
        gameObject.SetActive(false);
    }
}
