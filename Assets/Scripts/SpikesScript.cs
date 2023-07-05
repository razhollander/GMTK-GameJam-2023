using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    public bool can_kill = false;
    public float time_to_pop;
    SpriteRenderer renderer;
    bool player_inside = false;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_inside = true;
            StartCoroutine(initilize_spikes());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player_inside = false;
        }
    }

    IEnumerator initilize_spikes()
    {
        yield return new WaitForSeconds(time_to_pop);
        renderer.sprite = null;
        if (player_inside)
        {
            // something
        }
    }
}
