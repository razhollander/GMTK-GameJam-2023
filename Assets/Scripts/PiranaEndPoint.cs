using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranaEndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponentInParent<PiranaScript>())
        {
            GetComponentInParent<PiranaScript>().returning = true;
        }
    }
}
