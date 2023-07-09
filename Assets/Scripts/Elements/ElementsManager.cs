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
    [SerializeField] private float _upOffset = .6f;

    private void Awake()
    {
        Instance = this;
    }

    public void SetElement(ElementEffect elementEffect)
    {
        _currentElement = elementEffect;
        LandClickedManager.Instance.ReceiveElement(_currentElement);
    }

    public void CreateMeteors(Land land)
    {
        foreach (var neighbor in land.Neighbors)
        {
            var newMeteor = Instantiate(_meteorElement, neighbor.Position * _meteorSpawnRadius +
                                                        neighbor.transform.up * _upOffset, Quaternion.identity);
            newMeteor.SetLand(neighbor);
        }
        
        
    }
    
    public async UniTask CreateTornado(Land land)
    {
        var inst = Instantiate(_tornadoElement);
        inst.transform.position = Vector3.zero;
        inst.transform.rotation = Quaternion.Euler(Vector3.zero);
        inst.transform.position = land.Position;
        inst.SetFirstLand(land);
    }
}
