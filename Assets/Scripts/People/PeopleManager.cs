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
    [SerializeField] private List<Material> humanSkins;
    private PlanetManager _planetManager;
    private List<Land> groundLands;
    private bool _gameOver;
    #region DUBUG

    [SerializeField] private bool isDebug;
    [SerializeField] private int maxNum = 1;
    private int curNum = 0;
    #endregion
    private void Start()
    {
        _planetManager = PlanetManager.Instance;
        groundLands = _planetManager.Lands;
        GameManager.Instance.Loose += ToggleGameOver;
        Spawn();
    }

    private async UniTask Spawn()
    {
        if(_gameOver) return;
        await UniTask.Delay(TimeSpan.FromSeconds(timeBtwSpawns));
        curNum++;
        var groundRnd = Random.Range(0, groundLands.Count);
        var newPos = groundLands[groundRnd].Position;
        // var newPerson = Instantiate(person, newPos, quaternion.Euler(-newPos.x, -newPos.y, -newPos.z));
        var newPerson = Instantiate(person, newPos, quaternion.identity, PlanetManager.Instance.transform);
        while (groundLands[groundRnd].Vertex == 5)
        {
            groundRnd = Random.Range(0, groundLands.Count);
        }
        newPerson.FirstSetTarget(groundLands[groundRnd]);
        
        var materialRnd = Random.Range(0, 10);
        newPerson.SetLook(humanSkins[materialRnd]);
        if (!isDebug && curNum != maxNum) Spawn();
    }

    private void ToggleGameOver()
    {
        _gameOver = true;
    }

}
