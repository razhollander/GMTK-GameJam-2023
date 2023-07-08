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
    [SerializeField] private int _addedYOffset=1;
    [SerializeField] private float _startingYOffset=1;
    [SerializeField] private Color _colorPositive = Color.green;
    [SerializeField] private Color _colorNegative = Color.red;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    
    public async UniTask Play(int heatAmount)
    {
        Debug.Log("play");
        gameObject.SetActive(true);
        //_text.transform.LookAt(Camera.main.transform);
        //var zAngleRotation = (Quaternion.FromToRotation(transform.up, _text.transform.position) * transform.rotation).eulerAngles.z;
        //_text.transform.rotation = Quaternion.Euler(0, 0, zAngleRotation);
        RotateTextToCamera();
        _text.text =  heatAmount > 0? "+"+heatAmount.ToString()+"°" : heatAmount.ToString()+"°";
        _text.color = heatAmount > 0 ? _colorNegative : _colorPositive;
        _text.transform.localPosition = _text.transform.up * _startingYOffset;
        _text.transform.DOLocalMoveY(_text.transform.localPosition.y + _addedYOffset, _animationTime).OnComplete(OnCompleteEffect).SetEase(Ease.OutCubic);
        _text.DOFade(0, _animationTime).SetEase(Ease.InQuart);
    }

    private void RotateTextToCamera()
    {
        // _text.transform.rotation = Camera.main.transform.rotation;
        // Vector3 objectNormal = _text.transform.rotation * Vector3.forward;
        // Vector3 cameraToText = _text.transform.position - Camera.main.transform.position;
        // float f = Vector3.Dot (objectNormal, cameraToText);
        // if (f < 0f) 
        // {
        //     _text.transform.Rotate (0f, 180f, 0f);
        // }
        
        _text.transform.rotation = Quaternion.LookRotation((_text.transform.position - Camera.main.transform.position).normalized);
    }

    private void OnCompleteEffect()
    {
        gameObject.SetActive(false);
    }
}
