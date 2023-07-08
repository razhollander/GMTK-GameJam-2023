using System.Collections.Generic;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        [SerializeField] private GameObject _sea;
        [SerializeField] private GameObject _land;
        private int _amountNeighbors;
        
        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }

        public int AmountNeighbors
        {
            get => _amountNeighbors;
            set
            {
                _amountNeighbors = value;
                if (_amountNeighbors == 5)
                {
                    _sea.SetActive(true);
                }
                else
                {
                    _land.SetActive(true);
                }
            }
        }

        private void OnMouseEnter()
        {
            Debug.Log("Mouse enter");
        }

        private void OnMouseExit()
        {
            Debug.Log("Mouse exit");
        }

        private void OnMouseUpAsButton()
        {
            Debug.Log("Clicked");
        }
    }
}