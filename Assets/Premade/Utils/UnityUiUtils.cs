using UnityEngine;

namespace Utils
{
    public static class UnityUiUtils
    {
        private const float Half = 0.5f;
    
        public static Vector2 ConvertWorldPositionToAnchoredPositionOnCanvas(Vector3 worldPosition, RectTransform canvasRectTransform)
        {
            var viewportPosition = Camera.main.WorldToViewportPoint( worldPosition);
            var canvasSizeDelta = canvasRectTransform.sizeDelta;
            var worldObjectScreenPosition = new Vector2(viewportPosition.x * canvasSizeDelta.x, viewportPosition.y * canvasSizeDelta.y);
            var canvasHalfSize = new Vector2(canvasSizeDelta.x, canvasSizeDelta.y) * Half;
            var uiAnchoredPosition = worldObjectScreenPosition - canvasHalfSize;
        
            return uiAnchoredPosition;
        }
    }
}
