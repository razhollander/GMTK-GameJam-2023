using System.Collections.Generic;
using System.Linq;
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
                land.Neighbors = Lands.OrderBy(i => Vector3.Distance(i.Position, land.Position)).Where(i => i != land).Take(land.AmountNeighbors).ToList();
                float minimumDistance = 9999f;
                foreach (var neighbor in land.Neighbors)
                {
                    var distance = Vector3.Distance(land.Position, neighbor.Position);
                    if (distance < minimumDistance)
                    {
                        minimumDistance = distance;
                    }
                }

                land.Neighbors.RemoveAll(i => minimumDistance + 0.2f < Vector3.Distance(land.Position, i.Position));
            }
        }
    }
}