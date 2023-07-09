using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Toggle _toggleMusic;

        private void Awake()
        {
            _startButton.onClick.AddListener(()=> SceneManager.LoadScene("Game"));
            _toggleMusic.isOn = AudioListener.volume > 0.5f;
            _toggleMusic.onValueChanged.AddListener(i =>
            {
                if (i)
                {
                    AudioListener.volume = 1f;
                }
                else
                {
                    AudioListener.volume = 0f;
                }
            });

            AudioManager.Instance.Play(AudioManager.SoundsType.BackgroundMusic, true, true);
        }
    }
}
