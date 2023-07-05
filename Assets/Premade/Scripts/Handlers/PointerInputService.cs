using UnityEngine;
using UnityEngine.EventSystems;


    public class PointerInputService
    {
        public bool IsPointerOverGUI()
        {
            var isPointerOverGUI = false;

            isPointerOverGUI = EventSystem.current.IsPointerOverGameObject();

            return isPointerOverGUI;
        }
    }

