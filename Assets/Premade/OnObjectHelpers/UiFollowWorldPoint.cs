using Sirenix.OdinInspector;
using UnityEngine;
using Utils;


    public class UiFollowWorldPoint : MonoBehaviour
    {
        [SerializeField] protected int _xOffset;
        [SerializeField] private GameObject _objectToFollow;
        [SerializeField] private int _yOffset;

        private bool _objectToFollowSet;
        private int _screenWidth;
        private RectTransform _canvasRect;
        private RectTransform _rect;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            _canvasRect = (RectTransform) GetComponentInParent<Canvas>().gameObject.transform;
        }

        public void LateUpdate()
        {
            if (!_objectToFollowSet)
            {
                return;
            }

            UpdatePosition();
        }

        public void SetXOffset(int xMove)
        {
            _xOffset = xMove;
        }

        public void SetYOffset(int yMove)
        {
            _yOffset = yMove;
        }

        protected void SetObjectToFollow(GameObject gameObjectToFollow)
        {
            _objectToFollow = gameObjectToFollow;
            _objectToFollowSet = true;
        }

        [Button]
        private void UpdatePosition()
        {
            if (_objectToFollow == null)
            {
                return;
            }

            var offset = new Vector2(_xOffset, _yOffset);
            _rect.anchoredPosition = offset + UnityUiUtils.ConvertWorldPositionToAnchoredPositionOnCanvas(_objectToFollow.transform.position, _canvasRect);
        }
    }
