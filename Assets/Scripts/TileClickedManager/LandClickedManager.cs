using System;
using System.Collections;
using System.Collections.Generic;
using Planet;
using UnityEngine;

public class LandClickedManager : MonoBehaviour
{
    public static LandClickedManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LandMouseExit(Land land)
    {
        Debug.Log(nameof(LandMouseExit));
    }

    public void LandMouseUp(Land land)
    {
        Debug.Log(nameof(LandMouseUp));
    }

    public void LandMouseDown(Land land)
    {
        Debug.Log(nameof(LandMouseDown));
    }
}
