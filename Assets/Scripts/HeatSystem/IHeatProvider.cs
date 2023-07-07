using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeatProvider
{
    int HeatProvided { get; }
    void OnHeatInterval();
}
