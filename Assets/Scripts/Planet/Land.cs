using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        private int _amountNeighbors;
        private Material _material;
        private int _level;
        private BuildingType _buildingType;

        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }
        public Vector3 Position { get; set; }
        public int Vertex { get; set; }

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
    }
}