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

    private void Start()
    {
        _planetManager = PlanetManager.Instance;
        SpawnStar();
    }

    private async UniTask SpawnStar()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(timeBtwSpawns));
        print("spawn");
        var land = _planetManager.Lands[Random.Range(0, _planetManager.Lands.Count)];
        while (land.Vertex == 5)
        {
            land = _planetManager.Lands[Random.Range(0, _planetManager.Lands.Count)];
        }

        var newStar = Instantiate(star, land.Position * radius, Quaternion.identity);
        newStar.SetEffect(Random.Range(0,2));
        newStar.SetLand(land);
        SpawnStar();
    }
}

public enum ElementEffect
{
    None,
    Meteor,
    Tornado
}
