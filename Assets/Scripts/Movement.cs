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
    private grapplinghook gph;

    public bool goingup;
    public bool goingdown;
    public bool goingleft;
    public bool goingright;
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
    public SetImprovedCursor sic;

    // Start is called before the first frame update
    void Start()
    {
        activeMoveSpeed = speed;
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        armpos = new Vector2(arm.transform.position.x, arm.transform.position.y);
        newarmpos = new Vector2(arm.transform.position.x + 1, arm.transform.position.y + 1);
        gph = GetComponent<grapplinghook>();
        DashAnimation.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = Input.GetAxisRaw("Horizontal");
        if (gph.isHooked == false)
        {
            rb.velocity = new Vector2(x * activeMoveSpeed, rb.velocity.y);
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (isTouchingGround == false)
                {
                    rb.AddForce(Vector2.right * activeMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    rb.velocity = new Vector2(x * activeMoveSpeed, rb.velocity.y);
                }
                if (facingRight && isTouchingGround == false)
                {
                    DashAnimation.SetActive(true);
                }
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                rb.velocity = new Vector2(x * 0, rb.velocity.y);
                DashAnimation.SetActive(false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (isTouchingGround == false)
                {
                    rb.AddForce(Vector2.left * activeMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    rb.velocity = new Vector2(x * activeMoveSpeed, rb.velocity.y);
                }
                if (!facingRight && isTouchingGround == false)
                {
                    DashAnimation.SetActive(true);
                }
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                rb.velocity = new Vector2(x * 0, rb.velocity.y);
                DashAnimation.SetActive(false);
            }
        }

        if (rb.velocity.x > 0)
        {
            goingright = true;
            goingleft = false;
        }
        else if (rb.velocity.x < 0)
        {
            goingright = false;
            goingleft = true;
        }
        else
        {
            goingright = false;
            goingleft = false;
        }

        StartCoroutine(calcMovement());

        //https://www.youtube.com/watch?v=xHargJbONls&ab_channel=Antarsoft
        anim.SetBool("Jump", !isTouchingGround);
        anim.SetFloat("yVelocity", rb.velocity.y);


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

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) && gph.isHooked == false)
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




        if (isDashing)
        {
            /////////////////////////////////////////////////////
            /*http://answers.unity.com/answers/633909/view.html*/
            /////////////////////////////////////////////////////

            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //var targetPos = sic.transform.position;
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
        rb.constraints = RigidbodyConstraints2D.FreezePositionY; rb.constraints = RigidbodyConstraints2D.None; rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0;
        dashCooldownCounter = dashcoolDown;
        float tempspeed = speed;
        activeMoveSpeed = dashSpeed;
        isDashing = true;
        yield return new WaitForSeconds(dashLength);
        activeMoveSpeed = tempspeed;
        rb.gravityScale = normalgravity;
        isDashing = false;
        arm.transform.localPosition = new Vector2(-0.125f, -0.09375f);
        anim.SetBool("Running", false);
        DashAnimation.SetActive(false);
        yield return new WaitForSeconds(dashcoolDown);
        dashCooldownCounter = 0;
    }
    public void childtrigger()
    {
        isTouchingGround = !isTouchingGround;
    }

    IEnumerator calcMovement()
    {
        Vector3 prevPos = gameObject.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 actualPos = gameObject.transform.position;

        if (prevPos.y < actualPos.y && (actualPos.y - prevPos.y > 0.1))
        {
            goingup = true;
            goingdown = false;
        }
        else if (prevPos.y > actualPos.y && (prevPos.y - actualPos.y > 0.1))
        {
            goingup = false;
            goingdown = true;
        }
        else
        {
            goingup = false;
            goingdown = false;
        }

        if (prevPos.x < actualPos.x && (actualPos.x - prevPos.x > 0.1))
        {
            goingright = true;
            goingleft = false;
        }
        else if (prevPos.x > actualPos.x && (prevPos.x - actualPos.x > 0.1))
        {
            goingright = false;
            goingleft = true;
        }
        else
        {
            goingright = false;
            goingleft = false;
        }

    }
}
