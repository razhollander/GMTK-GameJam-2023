using UnityEngine;

namespace Planet
{
    public class PlanetExplodeManager : MonoBehaviour
    {
        [SerializeField] private GameObject _explode;
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
                PlanetManager.Instance.gameObject.SetActive(false);
            };
        }
    }
}