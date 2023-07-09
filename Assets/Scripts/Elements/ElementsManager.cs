using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Elements;
using Planet;
using UnityEngine;

public class ElementsManager : MonoBehaviour
{
    [SerializeField] private MeteorElement _meteorElement;
    [SerializeField] private TornadoElement _tornadoElement;
    [SerializeField] private float _meteorSpawnRadius = 6;

    private ElementEffect _currentElement = ElementEffect.None;

    public static ElementsManager Instance;
    [SerializeField] private float _upOffset = 1f;

    private void Awake()
    {
        Instance = this;
    }

    public void SetElement(ElementEffect elementEffect)
    {
        _currentElement = elementEffect;
        LandClickedManager.Instance.ReceiveElement(_currentElement);
    }

    public void CreateMeteor(Land land)
    {
        var newMeteor = Instantiate(_meteorElement, land.Position * _meteorSpawnRadius+Vector3.up*_upOffset, Quaternion.identity);
        newMeteor.SetLand(land);
    }
    
    public async UniTask CreateTornado(Land land)
    {
        var inst = Instantiate(_tornadoElement);
        inst.SetFirstLand(land);
        inst.transform.position = Vector3.zero;
        inst.transform.rotation = Quaternion.Euler(Vector3.zero);
        inst.transform.position = land.Position;
        inst.transform.LookAt(land.transform);
    }
}
