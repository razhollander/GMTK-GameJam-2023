using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string SCORE = "Score";
    private const string SAMPLE_SCENE_NAME = "SampleScene";
    private string saveScore = "ScoreSaved";

    public PlayerMovement player;
    public static GameManager Instance;
    public CameraManager CameraManager;
    public GameEventsSystem GameEventsSystem;
    public CardsManager CardManager;
    public Canvas canvas;
    public GameScoreText gameScoreText;
    public GameScoreText gameWinnerText;

    [SerializeField] private GameObject winnerPanel;
    [SerializeField] public MoneyManager MoneyManager;
    private bool isFlagGameOver = false;
    float _prevTimeScale = 1;

    private void Awake()
    {
        Instance = this;
        
        SetupSystems();
        StartCoroutine(Wait10Sec());
    }

    private IEnumerator Wait10Sec()
    {
        yield return new WaitForSeconds(10);
        CardManager.GenerateRandomCard();
    }
    
    private void OnEnable()
    {
        Instance = this;
    }

    private void SetupSystems()
    {
        CameraManager = new CameraManager();
        GameEventsSystem = new GameEventsSystem();
    }
    
    public void SaveHighScore()
    {
        var currentMaxScore = SaveLocallyHandler.LoadInt(SCORE);
        var currentGameScore = MoneyManager.Money;
        
        if (currentGameScore > currentMaxScore)
        {
            SaveLocallyHandler.SaveInt(SCORE, currentGameScore);
        }
    }
    
    public void PauseGame()
    {
        _prevTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Pool.pools = new Dictionary<PooledMonobehaviour, Pool>();
        SceneManager.LoadScene(SAMPLE_SCENE_NAME);
        Debug.Log("Restart");
    }

    public void GameOver()
    {
        if (!isFlagGameOver)
        {
            isFlagGameOver = true;
            player.lose();
            Time.timeScale = 0;
            SaveHighScore();
            canvas.GetComponent<Animator>().SetBool("GameOver", true);
            SaveHighScore();
            gameScoreText.SetText();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Win()
    {
            Time.timeScale = 0;
            winnerPanel.SetActive(true);
            SaveHighScore();
            gameWinnerText.SetText();
    }
}
