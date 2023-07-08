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
        public Vector3 Position { get; set; }

        public float LandView
        {
            set { _material.SetFloat("_Color", value); }
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