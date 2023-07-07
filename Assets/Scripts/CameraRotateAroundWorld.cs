using UnityEngine;

public class CameraRotateAroundWorld : MonoBehaviour
{
    [SerializeField] private Transform target; // The object to rotate around
    [SerializeField] private float rotationSpeed = 5f;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - lastMousePosition;

            float rotationX = mouseDelta.y * rotationSpeed;
            float rotationY = -mouseDelta.x * rotationSpeed;

            // Rotate the camera around the target object
            transform.RotateAround(target.position, Vector3.up, rotationY);
            transform.RotateAround(target.position, transform.right, rotationX);

            lastMousePosition = currentMousePosition;
        }
    }
}