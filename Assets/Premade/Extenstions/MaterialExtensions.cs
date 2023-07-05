using DG.Tweening;
using UnityEngine;

namespace CoreDomain.Scripts.Extensions
{
    public static class MaterialExtensions
    {
        public static void SetBool(this Material material, int shaderId, bool isActive)
        {
            material.SetFloat(shaderId, GetBoolShaderFloatValue(isActive));
        }

        public static void SetBoolWithTween(this Material material, int shaderId, bool isActive, float duration)
        {
            material.DOFloat(GetBoolShaderFloatValue(isActive), shaderId, duration);
        }

        public static bool GetBool(this Material material, int shaderId)
        {
            var materialFloatValue = material.GetFloat(shaderId);
            return materialFloatValue != 0f;
        }

        private static float GetBoolShaderFloatValue(bool isActive)
        {
            return isActive ? 1f : 0f;
        }
    }
}