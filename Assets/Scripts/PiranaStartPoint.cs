using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranaStartPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<PiranaScript>())
        {
            GetComponentInParent<PiranaScript>().returning = false;
        }
    }
}
