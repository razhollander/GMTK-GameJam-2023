using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Animator _animator;

    public static LoseUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        _scoreText.text = "Your Score: " + Score.Instance.ScoreAmount;
        _animator.Play("Show");
    }

    public void GoToMenu()
    {
        GameManager.Instance.GoToMenu();
    }
    
    public void Restart()
    {
        GameManager.Instance.Restart();
    }
}
