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
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public async UniTask Play(SoundsType type, bool isDontDestroyOnChangeScene = false)
        {
            var source = Instantiate(_audioSourcePrefab);
            source.clip = _clips.Find(t => t.Type == type).Clip;
            source.Play();

            if (isDontDestroyOnChangeScene)
            {
                DontDestroyOnLoad(source);
            }

            await UniTask.DelayFrame(3);

            while (source.isPlaying)
            {
                await UniTask.Delay(1000);
            }
            
            Destroy(source.gameObject);
        }
        
        public enum SoundsType
        {
            StartCollect,
            PoofLevelUpBuilding,
            BuildingDestroySuccess,
            HitBuilding,
            BackgroundMusic,
            Tornado,
            EarthExplode
        }
        
        [Serializable]
        private struct AudioPerId
        {
            public SoundsType Type;
            public AudioClip Clip;
        }
    }
}