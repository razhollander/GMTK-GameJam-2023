using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";
    
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _scoreSpeed = 10;
    private float _score;
    public int ScoreAmount => (int) _score;
    public int HighScoreAmount => PlayerPrefs.GetInt(HIGH_SCORE_KEY);

    public static Score Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        _score += Time.deltaTime * _scoreSpeed;
        _text.text = "Score: "+ScoreAmount.ToString();
    }

    public void SaveHighScore()
    {
        if (HighScoreAmount < ScoreAmount)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, ScoreAmount);
        }
    }
}
