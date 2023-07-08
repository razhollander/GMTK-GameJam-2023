using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Planet;
using UnityEngine;

public class ElementsManager : MonoBehaviour
{
    [SerializeField] private MeteorElement _meteorElement;
    [SerializeField] private float _meteorSpawnRadius = 6;

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
        //var newMeteor = Instantiate(_meteorElement, land.Position * _meteorSpawnRadius, Quaternion.identity);
        //newMeteor.SetLand(land);
    }
    
    public async UniTask CreateTornado()
    {
        CreateReleaseTornado();
    }
    
    private void CreateReleaseTornado()
    {
        
    }
}
