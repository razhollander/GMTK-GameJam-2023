using System;
using DG.Tweening;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private Countable MoneyText;

    public int Money = 0;
    Loot[] allLoot;
    public void Start()
    {
        allLoot = GameObject.FindObjectsOfType<Loot>();
    }

    public void AddMoney(int addedValue)
    {
        Money += addedValue;
        MoneyText.SetNumber(Money);
        allLoot = GameObject.FindObjectsOfType<Loot>();
Debug.Log(allLoot.Length.ToString());
        if (!allLoot.TryFind(x=>x.IsGiveAlready==false, out var _))
            GameManager.Instance.Win();
    }
}