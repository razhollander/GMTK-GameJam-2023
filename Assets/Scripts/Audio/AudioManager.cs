using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioPerId> _clips;
        [SerializeField] private AudioSource _audioSourcePrefab;
        
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public async UniTask Play(SoundsType type)
        {
            var source = Instantiate(_audioSourcePrefab);
            source.clip = _clips.Find(t => t.Type == type).Clip;
            source.Play();

            while (source.isPlaying)
            {
                await UniTask.Delay(1000);
            }
            
            Destroy(source);
        }
        
        public enum SoundsType
        {
            StartCollect
        }
        
        [Serializable]
        private struct AudioPerId
        {
            public SoundsType Type;
            public AudioClip Clip;
        }
    }
}