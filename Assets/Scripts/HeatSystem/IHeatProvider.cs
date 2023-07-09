using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeatProvider
{
    float HeatProvided { get; }
    void OnHeatInterval();
}
