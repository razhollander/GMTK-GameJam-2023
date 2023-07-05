using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var guard = GetComponentInParent<GuardScript>();

        if (guard != null)
        {
            //if (guard.transform == transform.parent)
            //{
                guard.returning = true;
            //}
        }
    }
}
