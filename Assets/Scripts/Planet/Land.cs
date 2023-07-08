using System;
using System.Collections.Generic;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        private Renderer _renderer;
        private int _amountNeighbors;
        
        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }

        public int AmountNeighbors
        {
            get => _amountNeighbors;
            set
            {
                _amountNeighbors = value;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnMouseEnter()
        {
            _renderer.sharedMaterial.SetFloat("OutLineOpacity", 1f);
        }

        private void OnMouseExit()
        {
            _renderer.sharedMaterial.SetFloat("OutLineOpacity", 0f);
        }

        private void OnMouseUpAsButton()
        {
            Debug.Log("Clicked");
        }
    }
}