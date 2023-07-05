using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraLightScript : MonoBehaviour
{

    UnityEngine.Rendering.Universal.Light2D light;

    private void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            light.color = Color.red;
        }
    }
}
