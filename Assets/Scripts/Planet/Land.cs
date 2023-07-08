using System.Collections.Generic;
using UnityEngine;

namespace Planet
{
    public class Land : MonoBehaviour
    {
        public int Id { get; set; }
        public List<Land> Neighbors { get; set; }
        public int AmountNeighbors { get; set; }
        
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