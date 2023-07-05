using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranaCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.GameOver();
        }
    }
}
