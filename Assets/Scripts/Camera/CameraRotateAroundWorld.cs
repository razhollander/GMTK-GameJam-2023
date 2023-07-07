using UnityEngine;

public class CameraRotateAroundWorld : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 3f;
    private Vector3 _lastTouch;
    private Vector3 _centerLook;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            var deltaTouch = Input.mousePosition - _lastTouch;
            _lastTouch = Input.mousePosition;
            
            float angelX = deltaTouch.x * Time.deltaTime * _speed;
            float angelY = deltaTouch.y * Time.deltaTime * _speed * -1;
            
            RotateAround(_target.position, new Vector3(0,1,0), angelX);
            RotateAround(_target.position, new Vector3(1,0,0), angelY);
            
            transform.LookAt(_target.position);
        }

        _lastTouch = Input.mousePosition;
    }

    private void RotateAround(Vector3 center, Vector3 axis, float angel)
    {
        Vector3 correctAxis;

        if (axis == Vector3.right)
        {
            correctAxis = transform.right;
        }
        else if (axis == Vector3.left)
        {
            correctAxis = -transform.right;
        }
        else
        {
            correctAxis = axis;
        }

        transform.RotateAround(center, correctAxis, angel);
    }
}