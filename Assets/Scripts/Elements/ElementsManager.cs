using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsManager : MonoBehaviour
{
    private ElementEffect _currentElement = ElementEffect.None;

    public static ElementsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetElement(ElementEffect elementEffect)
    {
        _currentElement = elementEffect;
        LandClickedManager.Instance.ReceiveElement(_currentElement);
    }

    public void CreateMeteor()
    {
        
    }
    
    public async UniTask CreateTornado()
    {
        CreateReleaseTornado();
    }
    
    private void CreateReleaseTornado()
    {
        
    }
}
