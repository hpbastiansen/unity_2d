using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]

    public Rigidbody2D rb;
    Animator anim;
    public float speed = 10;
    public int normalgravity = 10;
    public int downwardspeed = 10;
    public float jumpforce = 5;
    Vector2 move;
    public bool facingRight = true;

    [Header("GroundCheck")]

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isTouchingGround;
    //Dash
    [Header("Dash")]
    public float dashSpeed;
    private float activeMoveSpeed;
    public float dashLength = .5f, dashcoolDown = 1f;
    private float dashCounter;
    private float dashCooldownCounter;

    public Collider2D groundcol;

    // Start is called before the first frame update
    void Start()
    {
        activeMoveSpeed = speed;
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = Input.GetAxisRaw("Horizontal");


        rb.velocity = new Vector2(x * activeMoveSpeed, rb.velocity.y);
        if (mousePos.x < transform.position.x && facingRight)
        {
            flip();
        }
        else if (mousePos.x > transform.position.x && !facingRight)
        {
            flip();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            if (isTouchingGround)
            {
                //Important, because when jumping while player already got an acting force in the Y-axis they will combine to give the player super jump. This prevents it.
                rb.constraints = RigidbodyConstraints2D.FreezePositionY; rb.constraints = RigidbodyConstraints2D.None; rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.AddForce(new Vector2(0, jumpforce), ForceMode2D.Force);

            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.gravityScale += 10;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            rb.gravityScale = normalgravity;

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashCooldownCounter <= 0)
            {
                StartCoroutine(Dash());
            }
        }

    }
    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    IEnumerator Dash()
    {
        dashCooldownCounter = dashcoolDown;
        float tempspeed = speed;
        activeMoveSpeed = dashSpeed;
        yield return new WaitForSeconds(dashLength);
        activeMoveSpeed = tempspeed;
        yield return new WaitForSeconds(dashcoolDown);
        dashCooldownCounter = 0;
    }
    public void childtrigger()
    {
        isTouchingGround = !isTouchingGround;
    }
}
