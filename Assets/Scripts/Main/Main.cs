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

        [SerializeField] private Image volume;
        [SerializeField] private Sprite on;
        [SerializeField] private Sprite off;
        [SerializeField] private CanvasGroup group;
        private bool tutorialOn;
        private void Awake()
        {
            _startButton.onClick.AddListener(()=> SceneManager.LoadScene("Game"));
            _toggleMusic.isOn = AudioListener.volume > 0.5f;
            _toggleMusic.onValueChanged.AddListener(i =>
            {
                if (i)
                {
                    AudioListener.volume = 1f;
                    volume.sprite = on;
                }
                else
                {
                    AudioListener.volume = 0f;
                    volume.sprite = off;

                }
            });

            AudioManager.Instance.Play(AudioManager.SoundsType.BackgroundMusic, true, true);
        }

        public void ToggleTutorial()
        {
            tutorialOn = !tutorialOn;
            if (tutorialOn)
            {
                group.alpha = 1;
            }
            else
            {
                group.alpha = 0;
            }
        }
    }
}
