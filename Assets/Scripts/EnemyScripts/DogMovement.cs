using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : GaneEventListener
{
    private Transform player;

    [SerializeField]
    private int speed;
    private int initial_speed;
    [SerializeField]
    private int max_dis;
    Rigidbody2D rb;
    [SerializeField] private Animator _anim;
    
    private Vector3 startLocalScale;
    public override void OnGameEvent(GameEvent gameEvent)
    {
        speed = initial_speed;

        if (gameEvent == GameEvent.Alarm)
        {
            speed *= 2;
        }

        else if (gameEvent == GameEvent.Flood)
        {
            speed /= 2;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startLocalScale = transform.localScale;
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < max_dis)
        {
            Vector3 direction = player.position - transform.position;

            direction = direction.normalized;

            Vector3 velocity = direction * speed;
            rb.AddForce(velocity, ForceMode2D.Force);

        }
        
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-startLocalScale.x, startLocalScale.y,startLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(startLocalScale.x, startLocalScale.y,startLocalScale.z);

        }

        if (rb.velocity.sqrMagnitude > 0.4f)
        {
            _anim.SetBool("Move",true);
        }
        else
        {
            _anim.SetBool("Move",false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.GameOver();
        }
    }

}
