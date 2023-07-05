using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStartPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<GuardScript>())
        {
            GetComponentInParent<GuardScript>().returning = false;
        }
    }
}
