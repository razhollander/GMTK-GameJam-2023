using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Planet
{
    public class PlanetManager : MonoBehaviour
    {
        public static PlanetManager Instance { get; private set; }
        public List<Land> Lands { get; private set; }

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            CreateParts();
        }

        private void CreateParts()
        {
            var parts = GetComponentsInChildren<LandPositionGetter>();
            Lands = new();
            int idCounter = 0;
            foreach (var part in parts)
            {
                var land = part.gameObject.GetComponent<Land>();
                Lands.Add(land);
                land.Id = idCounter;
                land.AmountNeighbors = part.Vertex;
                land.Position = part.Position;
                idCounter++;
            }

            foreach (var land in Lands)
            {
                land.Neighbors = Lands.OrderBy(i => Vector3.Distance(i.transform.position, land.transform.position)).Take(land.AmountNeighbors + 1).TakeLast(land.AmountNeighbors).ToList();
            }
        }
    }
}