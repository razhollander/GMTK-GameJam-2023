
#if UNITY_EDITOR

using System.Collections;

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.Serialization;
#endif
using UnityEngine;

namespace Oprimization
{
    public class MeshSimplify : MonoBehaviour
    {
        public float quality = 0.5f;

#if UNITY_EDITOR
        
        [Button]
        private void Simplify()
        {
            var originalMesh = GetComponent<MeshFilter>().sharedMesh;
            var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
            meshSimplifier.Initialize(originalMesh);
            meshSimplifier.SimplifyMesh(quality);
            var destMesh = meshSimplifier.ToMesh();
            GetComponent<MeshFilter>().sharedMesh = destMesh;
        }
#endif
    }
}