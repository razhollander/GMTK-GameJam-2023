using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeatIntervalObserver
{
    void OnHeatInterval(int newHeat, int deltaHeat);
}
