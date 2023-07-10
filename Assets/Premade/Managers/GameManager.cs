using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IHeatIntervalObserver
{
    public const string SCORE = "Score";
    private const string SAMPLE_SCENE_NAME = "SampleScene";
    private string saveScore = "ScoreSaved";

    public Action Loose;

    public static GameManager Instance;
    public CameraManager CameraManager;

    private bool _didLoseAlready = false;
    private LoseUI _loseUI;
    
    private void Awake()
    {
        Instance = this;
        SetupSystems();
    }

    private void Start()
    {
        HeatSystem.Instance.AddHeatIntervalObserver(this);
        _loseUI = LoseUI.Instance;
    }
    
    private void OnDestroy()
    {
        HeatSystem.Instance.RemoveHeatIntervalObserver(this);
    }

    private void OnEnable()
    {
        Instance = this;
    }

    private void SetupSystems()
    {
        CameraManager = new CameraManager();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnHeatInterval(float newHeat, float deltaHeat)
    {
        if (!_didLoseAlready && newHeat > 99.90f)
        {
            DoLose();
        }
    }

    private void DoLose()
    {
        _didLoseAlready = true;
        Loose?.Invoke();

        Score.Instance.SaveHighScore();
        Score.Instance.isDisabled = true;

        _loseUI.gameObject.SetActive(true);
        _loseUI.Show();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void Restart()
    {
        Pool.pools = new Dictionary<PooledMonobehaviour, Pool>();
        SceneManager.LoadScene("Game");
    }
}
