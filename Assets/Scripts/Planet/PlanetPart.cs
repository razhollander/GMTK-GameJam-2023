using System;
using UnityEngine;

namespace Planet
{
    public class PlanetPart : MonoBehaviour
    {
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