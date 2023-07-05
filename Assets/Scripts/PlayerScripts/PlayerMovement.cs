using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GaneEventListener
{



    [HideInInspector]
    public float MoveDirectionX;
    [HideInInspector]
    public float MoveDirectionY;

    [SerializeField] private Animator _anim;
    private Rigidbody2D rb;

    [SerializeField]
    private int Speed;
    private int init_Speed;
    [SerializeField]
    private int roll_speed;
    [SerializeField] private float roll_time;
    //private Animator animator;

    public bool rolling = false;
    [SerializeField] private float slideCooldownInSeconds =1;
    private bool can_dash = true;
    private Vector3 scaleStart;

    public override void OnGameEvent(GameEvent gameEvent)
    {
        Speed = init_Speed;
        if (gameEvent == GameEvent.Freeze)
        {
            Speed /= 2;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleStart = transform.localScale;
        init_Speed = Speed;
        //animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
    }

    void Move()
    {
        MoveDirectionX = Input.GetAxisRaw("Horizontal");
        MoveDirectionY = Input.GetAxisRaw("Vertical");
        rb.AddForce( new Vector2(MoveDirectionX * Speed, MoveDirectionY * Speed), ForceMode2D.Force);

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-scaleStart.x, scaleStart.y,scaleStart.z);
        }
        else
        {
            transform.localScale = new Vector3(scaleStart.x, scaleStart.y,scaleStart.z);

        }
        if (MoveDirectionX != 0 || MoveDirectionY != 0)
        {
            _anim.SetBool("Move", true);
        }
        else
        {
            _anim.SetBool("Move", false);
        }

        if (Input.GetKey(KeyCode.Space) && can_dash)
        {
            Roll();
        }
    }

    void Roll()
    {
        can_dash = false;
        rb.AddForce(new Vector2(MoveDirectionX, MoveDirectionY) * roll_speed, ForceMode2D.Impulse);
        StartCoroutine(start_dash()); 
        _anim.SetBool("Slide", true);
    }

    IEnumerator start_dash()
    {
        rolling = true;
        yield return new WaitForSeconds(roll_time);
        rolling = false;
        _anim.SetBool("Slide", false);
        StartCoroutine(coolDownDash());
    }

    IEnumerator coolDownDash()
    {
        yield return new WaitForSeconds(slideCooldownInSeconds);
        can_dash = true;
    }

    public void lose()
    {
        _anim.SetBool("Lose", true);
    }
    
}
