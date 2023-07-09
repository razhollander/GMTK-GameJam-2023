using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeatIntervalObserver
{
    void OnHeatInterval(float newHeat, float deltaHeat);
}
