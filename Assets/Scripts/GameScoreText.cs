using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScoreText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText()
    {
        var currentMaxScore = SaveLocallyHandler.LoadInt(GameManager.SCORE);
        var curScore = GameManager.Instance.MoneyManager.Money;
        var txt =  "Your Score: " + curScore;

        if (currentMaxScore > 0)
        {
            txt += ", Your Highscore: " + currentMaxScore;
        }

        text.text = txt;
    }
}
