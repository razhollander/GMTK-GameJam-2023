using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var m = LayerMask.NameToLayer("Player");
        if (collision.tag == "Player" && collision.gameObject.layer == m)
        {
            GameManager.Instance.GameOver();
        }
    }
}
