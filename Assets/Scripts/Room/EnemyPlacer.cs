using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour
{
    public List<Enemy> EnemiesCanBePlaced;
}

public enum Enemy
{
    Piranha = 0,
    Guard = 1,
    Projector = 2,
    Spike = 3,
    Dog = 4
}
