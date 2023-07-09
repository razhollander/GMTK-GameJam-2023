using UnityEngine;

namespace Planet
{
    public class PlanetExplodeManager : MonoBehaviour
    {
        [SerializeField] private GameObject _explode;
        [SerializeField] private ParticleSystem _particle;
        
        public static PlanetExplodeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            GameManager.Instance.Loose += () =>
            {
                _explode.SetActive(true);
                _particle.Play();
                PlanetManager.Instance.gameObject.SetActive(false);
            };
        }
    }
}