using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        private Renderer _renderer;
        private int _amountNeighbors;
        private Material _material;
         
        private int _level;

        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }
        public Vector3 Position { get; set; }

        public int Level
        {
            get { return _level;}
            set
            {
                _level = value;
                _material.SetFloat("_Color", value);
            }
        }
        
        public BuildingType BuildingType { get; set; }
        
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

        public void BuildForest(int level)
        {
            
        }
        
        public void BuildBuilding(int level)
        {
            
        }
        
        public void DestroyBuilding(int level)
        {
            
        }
        
        public void HitBuilding(int level)
        {
            
        }
    }
}