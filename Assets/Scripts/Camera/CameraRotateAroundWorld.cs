using UnityEngine;

namespace CameraGameJam
{
    public class CameraRotateAroundWorld : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _animationSmooth = 0.01f;
        private Vector3 _lastTouch;
        private Vector3 _centerLook;
        private Vector3 _lastDeltaTouch;

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                var deltaTouch = Input.mousePosition - _lastTouch;
                _lastDeltaTouch = deltaTouch;
                _lastTouch = Input.mousePosition;

                float angelX = deltaTouch.x * Time.deltaTime * _speed;
                float angelY = deltaTouch.y * Time.deltaTime * _speed * -1;

                RotateAround(_target.position, new Vector3(0, 1, 0), angelX);
                RotateAround(_target.position, new Vector3(1, 0, 0), angelY);
            }
            else
            {
                float angelX = _lastDeltaTouch.x * Time.deltaTime * _speed;
                float angelY = _lastDeltaTouch.y * Time.deltaTime * _speed * -1;

                RotateAround(_target.position, new Vector3(0, 1, 0), angelX);
                RotateAround(_target.position, new Vector3(1, 0, 0), angelY);

                _lastDeltaTouch.x = _lastDeltaTouch.x >= 0 ? _lastDeltaTouch.x - _animationSmooth : _lastDeltaTouch.x + _animationSmooth;
                _lastDeltaTouch.y = _lastDeltaTouch.y >= 0 ? _lastDeltaTouch.y - _animationSmooth : _lastDeltaTouch.y + _animationSmooth;
                _lastDeltaTouch.z = _lastDeltaTouch.z >= 0 ? _lastDeltaTouch.z - _animationSmooth : _lastDeltaTouch.z + _animationSmooth;

                if (Mathf.Abs(_lastDeltaTouch.x) <= _animationSmooth)
                {
                    _lastDeltaTouch.x = 0;
                }
                
                if (Mathf.Abs(_lastDeltaTouch.y) <= _animationSmooth)
                {
                    _lastDeltaTouch.y = 0;
                }
                
                if (Mathf.Abs(_lastDeltaTouch.z) <= _animationSmooth)
                {
                    _lastDeltaTouch.z = 0;
                }
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
            else if (axis == Vector3.up)
            {
                correctAxis = transform.up;
            }
            else if (axis == Vector3.down)
            {
                correctAxis = -transform.up;
            }
            else
            {
                correctAxis = axis;
            }

            transform.RotateAround(center, correctAxis, angel);
        }
    }
}