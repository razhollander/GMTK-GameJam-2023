using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        [SerializeField] private List<BuildingObjectsContainer> _buildingObjects;
        
        private int _amountNeighbors;
        private Material _material;
        private int _level;
        private BuildingType _buildingType;

        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }
        public Vector3 Position { get; set; }
        public int Vertex { get; set; }
        public int Heart { get; set; }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;

                UpdateColorView();
            }
        }

        public BuildingType BuildingType
        {
            get => _buildingType;
            set
            {
                _buildingType = value;
                UpdateColorView();
            }
        }

        private void UpdateColorView()
        {
            if (Vertex == 6)
            {
                switch (BuildingType)
                {
                    case BuildingType.None:
                        _material.SetFloat("_Color", 0.65f);
                        break;
                    case BuildingType.Forest:
                        _material.SetFloat("_Color", 0f);
                        break;
                    case BuildingType.Building:
                        if (Level == 4)
                        {
                            _material.SetFloat("_Color", 1f);
                        }
                        else if (_level == 3)
                        {
                            _material.SetFloat("_Color", 0.9f);
                        }
                        else if (_level == 2)
                        {
                            _material.SetFloat("_Color", 0.8f);
                        }
                        else if (_level == 1)
                        {
                            _material.SetFloat("_Color", 0.7f);
                        }
                        else
                        {
                            _material.SetFloat("_Color", 0.65f);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(BuildingType), BuildingType, null);
                }
            }
        }

        public int AmountNeighbors
        {
            get => _amountNeighbors;
            set { _amountNeighbors = value; }
        }

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void OnMouseEnter()
        {
            _material.SetFloat("_OutLineOpacity", 1f);
        }

        private void OnMouseExit()
        {
            LandClickedManager.Instance.LandMouseExit(this);
            _material.SetFloat("_OutLineOpacity", 0f);
        }

        private void OnMouseUpAsButton()
        {
            Debug.Log("Clicked");
        }

        private void OnMouseDown()
        {
            LandClickedManager.Instance.LandMouseDown(this);
        }

        private void OnMouseUp()
        {
            LandClickedManager.Instance.LandMouseUp(this);
        }

        public void BuildForest()
        {
            Build(1, BuildingType.Forest);
        }

        public void BuildBuilding(int level)
        {
            Build(level, BuildingType.Building);
        }

        private void Build(int level, BuildingType buildType)
        {
            var index = 0;
            Level = level;
            var builds = _buildingObjects.First(i => i.Type == buildType);
            foreach (var buildsObject in builds.Objects)
            {
                if (index + 1 == Level)
                {
                    buildsObject.SetActive(true);
                }
                else
                {
                    buildsObject.SetActive(false);
                }
            }
        }

        public void DestroyBuilding()
        {
            _buildingObjects.ForEach(i => i.Objects.ForEach(j => j.gameObject.SetActive(false)));
        }
        
        public void HitBuilding()
        {
            Heart--;
        }
        
        public void DestroyForest()
        {
            _buildingObjects.ForEach(i => i.Objects.ForEach(j => j.gameObject.SetActive(false)));
        }
        
        public void HitForest()
        {
            Heart--;
        }
        
        [Serializable]
        private struct BuildingObjectsContainer
        {
            public BuildingType Type;
            public List<GameObject> Objects;
        }
    }
}