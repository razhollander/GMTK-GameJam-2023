using System.Collections;
using System.Collections.Generic;
using Planet;
using UnityEngine;

public class LandWaterCenter : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private Land _land;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer.material.SetVector("_Center", _land.Position);
    }
}
