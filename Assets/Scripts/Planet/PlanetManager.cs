using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Planet
{
    public class PlanetManager : MonoBehaviour
    {
        public static PlanetManager Instance { get; private set; }
        public List<Land> Lands { get; private set; }

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
                land.Vertex = part.Vertex;
                land.BuildingType = BuildingType.None;
                idCounter++;
            }

            foreach (var land in Lands)
            {
                if (land.Vertex > 5)
                {
                    land.Neighbors = Lands.OrderBy(i => Vector3.Distance(i.Position, land.Position)).ToList();
                    land.Neighbors = land.Neighbors.Take(land.Vertex).ToList();
                    land.Neighbors = land.Neighbors.Where(i => i.Vertex == 6).ToList();
                }

            }

            // TempShowNeighbors();
        }

        private async UniTask TempShowNeighbors()
        {
            foreach (var land in Lands)
            {
                foreach (var neighbor in land.Neighbors)
                {
                    neighbor.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }

                await UniTask.Delay(2000);

                foreach (var neighbor in land.Neighbors)
                {
                    neighbor.transform.localScale = Vector3.one;
                }
            }
        }
    }
}