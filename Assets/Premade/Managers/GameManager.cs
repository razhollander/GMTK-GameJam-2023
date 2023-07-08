using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IHeatIntervalObserver
{
    public const string SCORE = "Score";
    private const string SAMPLE_SCENE_NAME = "SampleScene";
    private string saveScore = "ScoreSaved";

    public static GameManager Instance;
    public CameraManager CameraManager;
    
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

    public void OnHeatInterval(int newHeat, int deltaHeat)
    {
        if (newHeat == 100)
        {
            DoLose();
        }
    }

    private void DoLose()
    {
        Score.Instance.SaveHighScore();
        _loseUI.gameObject.SetActive(true);
        _loseUI.Show();
    }

    public void GoToMenu()
    {
        
    }
    
    public void Restart()
    {
        Pool.pools = new Dictionary<PooledMonobehaviour, Pool>();
        SceneManager.LoadScene(SAMPLE_SCENE_NAME);
    }
}
