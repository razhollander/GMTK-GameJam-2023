using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string SCORE = "Score";
    private const string SAMPLE_SCENE_NAME = "SampleScene";
    private string saveScore = "ScoreSaved";

    public static GameManager Instance;
    public CameraManager CameraManager;

    private void Awake()
    {
        Instance = this;
        SetupSystems();
    }

    private void OnEnable()
    {
        Instance = this;
    }

    private void SetupSystems()
    {
        CameraManager = new CameraManager();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Pool.pools = new Dictionary<PooledMonobehaviour, Pool>();
        SceneManager.LoadScene(SAMPLE_SCENE_NAME);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
