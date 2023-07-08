using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Planet;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeopleManager : MonoBehaviour
{
    [SerializeField] private PlayerController person;
    [SerializeField] private float timeBtwSpawns;

    private PlanetManager _planetManager;
    private List<Land> groundLands;

    private void Start()
    {
        _planetManager = PlanetManager.Instance;
        groundLands = _planetManager.Lands;
        Spawn();
    }

    private async UniTask Spawn()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(timeBtwSpawns));

        var rnd1 = Random.Range(0, groundLands.Count);
        var rnd2 = Random.Range(0, groundLands.Count);
        var newPos = groundLands[rnd1].transform.position;
        var newPerson = Instantiate(person, newPos, quaternion.Euler(-newPos.x, -newPos.y, -newPos.z));
        newPerson.SetTarget(groundLands[rnd2].transform);
        Spawn();
    }

    
}
