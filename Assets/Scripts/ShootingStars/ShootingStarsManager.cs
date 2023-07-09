using System;
using Cysharp.Threading.Tasks;
using Planet;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingStarsManager : MonoBehaviour
{
    private PlanetManager _planetManager;

    [SerializeField] private ShootingStar star;
    [SerializeField] private float radius = 6;
    [SerializeField] private float timeBtwSpawns = 5;
    private bool _gameOver;
    private void Start()
    {
        _planetManager = PlanetManager.Instance;
        GameManager.Instance.Loose += ToggleGameOver;
        SpawnStar();
    }

    private async UniTask SpawnStar()
    {
        if(_gameOver) return;
        await UniTask.Delay(TimeSpan.FromSeconds(timeBtwSpawns));
        var land = _planetManager.Lands[Random.Range(0, _planetManager.Lands.Count)];
        while (land.IsSea)
        {
            land = _planetManager.Lands[Random.Range(0, _planetManager.Lands.Count)];
        }

        var newStar = Instantiate(star, land.Position * radius, Quaternion.identity, PlanetManager.Instance.transform);
        newStar.SetLand(land);
        SpawnStar();
    }

    private void ToggleGameOver()
    {
        _gameOver = true;
    }
    
}

public enum ElementEffect
{
    None,
    Meteor,
    Tornado
}
