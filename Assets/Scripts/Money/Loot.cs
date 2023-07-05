using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _image;
    [SerializeField] private int _moneyAmount = 10;
    [SerializeField] private LootCollectEffect _collectEffect;

    [SerializeField] private Sprite SmallLootImage;
    [SerializeField] private Sprite MediumLootImage;
    [SerializeField] private Sprite BigLootImage;

    [SerializeField] private int _smallAmount = 100;
    [SerializeField] private int _mediumAmount = 500;
    [SerializeField] private int _bigAmount = 1000;
    [SerializeField] private Collider2D _collider;

    [HideInInspector] private bool isCollected = false;
    
    public bool IsCollected
    {
        get { return isCollected; }
    }
    private bool isGiveAlready = false;
    public bool IsGiveAlready
    {
        get { return isGiveAlready; }
    }
    private void Awake()
    {
        if (_moneyAmount < _mediumAmount)
        {
            _image.sprite = SmallLootImage;
        }
        else
        {
            if (_moneyAmount < _bigAmount)
            {
                _image.sprite = MediumLootImage;
            }
            else
            {
                if (_moneyAmount >= _bigAmount)
                {
                    _image.sprite = BigLootImage;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isGiveAlready)
        {
            isGiveAlready = true;
            isCollected = true;
            GameManager.Instance.MoneyManager.AddMoney(_moneyAmount);
            DoEffect();
        }
    }

    [ContextMenu("do effect")]
    private void DoEffect()
    {
        _image.enabled = false;
        _collider.enabled = false;
        _collectEffect.Play(_moneyAmount);
    }
    
}
