using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D rb;
    [SerializeField]
    public Animator anim;
    public float speed = 10;
    public int normalgravity = 10;
    public int downwardspeed = 10;
    public float jumpforce = 5;
    Vector2 move;
    public bool facingRight = true;
    public GameObject arm;
    private Vector2 armpos;
    private Vector2 newarmpos;
    [Header("GroundCheck")]

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isTouchingGround;
    //Dash
    [Header("Dash")]
    public bool isDashing = false;
    public float dashSpeed;
    public float activeMoveSpeed;
    public float dashLength = .5f, dashcoolDown = 1f;
    private float dashCounter;
    private float dashCooldownCounter;
    public GameObject DashAnimation;



    // Start is called before the first frame update
    void Start()
    {
        activeMoveSpeed = speed;
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        armpos = new Vector2(arm.transform.position.x, arm.transform.position.y);
        newarmpos = new Vector2(arm.transform.position.x + 1, arm.transform.position.y + 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = Input.GetAxisRaw("Horizontal");
        //https://www.youtube.com/watch?v=xHargJbONls&ab_channel=Antarsoft
        anim.SetBool("Jump", !isTouchingGround);
        anim.SetFloat("yVelocity", rb.velocity.y);
        if (isDashing == false)
        {
            rb.velocity = new Vector2(x * activeMoveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = rb.velocity;

        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && isTouchingGround == true && isDashing == false)
        {
            anim.SetBool("Running", true);
            arm.transform.localPosition = new Vector2(-0.0625f, -0.0625f);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            arm.transform.localPosition = new Vector2(-0.125f, -0.09375f);
            anim.SetBool("Running", false);

        }

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
                anim.SetBool("Jump", true);
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
        if (isDashing == true)
        {
            DashAnimation.SetActive(true);
            if (isTouchingGround == true)
            {
                anim.SetBool("Running", true);
                arm.transform.localPosition = new Vector2(-0.0625f, -0.0625f);
            }
            else
            {
                arm.transform.localPosition = new Vector2(-0.125f, -0.09375f);
                anim.SetBool("Running", false);
            }
        }
        else
        {
            DashAnimation.SetActive(false);

        }
        if (isDashing)
        {
            /////////////////////////////////////////////////////
            /*http://answers.unity.com/answers/633909/view.html*/
            /////////////////////////////////////////////////////

            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, activeMoveSpeed * Time.deltaTime);
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
        isDashing = true;
        yield return new WaitForSeconds(dashLength);
        activeMoveSpeed = tempspeed;
        isDashing = false;
        arm.transform.localPosition = new Vector2(-0.125f, -0.09375f);
        anim.SetBool("Running", false);
        yield return new WaitForSeconds(dashcoolDown);
        dashCooldownCounter = 0;
    }
    public void childtrigger()
    {
        isTouchingGround = !isTouchingGround;
    }
}
