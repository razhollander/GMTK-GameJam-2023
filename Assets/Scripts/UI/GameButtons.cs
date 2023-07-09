using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    [SerializeField] private Toggle _pauseToggle;
    [SerializeField] private Toggle _muteToggle;

    public static GameButtons Instance;
    public bool IsPaused { get; private set; }
    private void Awake()
    {
        Instance = this;
        _pauseToggle.onValueChanged.AddListener(Pause);
        _muteToggle.onValueChanged.AddListener(Mute);
    }

    private void OnDestroy()
    {
        _pauseToggle.onValueChanged.RemoveListener(Pause);
        _muteToggle.onValueChanged.RemoveListener(Mute);
    }

    private void Pause(bool isOn)
    {
        IsPaused = isOn;
        if (isOn)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    
    public void Restart()
    {
        SceneManager.LoadSceneAsync("Game");
    }
    
    private void Mute(bool isMute)
    {
        var volume = isMute? 0:1;
        AudioListener.volume = volume;
    }
}
