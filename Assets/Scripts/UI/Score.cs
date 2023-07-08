using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _scoreSpeed = 10;
    private float _score;
    public int ScoreAmount => (int) _score;

    public static Score Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        _score += Time.deltaTime * _scoreSpeed;
        _text.text = ScoreAmount.ToString();
    }
}
