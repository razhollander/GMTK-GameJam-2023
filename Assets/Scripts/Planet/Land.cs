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
            _material = GetComponent<Renderer>().material;
        }

        private void OnMouseEnter()
        {
            _material.SetFloat("_OutLineOpacity", 1f);
        }

        private void OnMouseExit()
        {
            LandClickedManager.Instance.LandMouseExit(this);
            Debug.Log("Mouse exit");
            _material.SetFloat("_OutLineOpacity", 0f);
        }

        private void OnMouseUpAsButton()
        {
            Debug.Log("Clicked");
        }

        private void OnMouseUp()
        {
            LandClickedManager.Instance.LandMouseUp(this);
        }

        private void OnMouseDown()
        {
            LandClickedManager.Instance.LandMouseDown(this);
        }
    }
}