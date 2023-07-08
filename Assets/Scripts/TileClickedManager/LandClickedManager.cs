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
        throw new NotImplementedException();
    }

    public void LandMouseUp(Land land)
    {
        throw new NotImplementedException();
    }

    public void LandMouseDown(Land land)
    {
        throw new NotImplementedException();
    }
}
