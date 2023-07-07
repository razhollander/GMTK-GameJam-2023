using UnityEngine;

namespace Planet
{
    public class PlanetManager : MonoBehaviour
    {
        [SerializeField] private PlanetPart _planetPart;

        public PlanetManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            CreateParts();
        }

        private void CreateParts()
        {
            var parts = GetComponentsInChildren<PartPositionGetter>();

            foreach (var part in parts)
            {
                Instantiate(_planetPart, part.Position, Quaternion.LookRotation(part.Position), transform);
                Destroy(part.gameObject);
            }
        }
    }
}