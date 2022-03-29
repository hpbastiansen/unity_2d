using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///The Movement script is an overall collection of player movement.
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D PlayerRigidbody;
    public Animator PlayerAnimator;
    public float MoveSpeed = 10;
    public int NormalGravity = 5;
    public float JumpForce = 5;
    public bool FacingRight = true;
    public GameObject ArmPivotGameObject;
    private Vector2 _armPosition;
    private Vector2 _armPositionRunning;
    private GrapplingHookController _grapplingHookController;
    public float ActiveMoveSpeed;
    public bool CanWalk;

    public bool GoingUp;
    public bool GoingDown;
    public bool GoingLeft;
    public bool GoingRight;
    [Header("GroundCheck")]

    public Transform GroundCheckObject;
    public float GroundCheckRadius;
    public bool IsTouchingGround;
    public bool IsCloseToRoof;
    private bool _tryingToJump;
    public bool PlayerIsInsideAnotherCollider;

    [Header("Dash")]
    public bool IsDashing = false;
    public float DashSpeed;

    public float DashLength = .5f, DashCooldown = 1f;
    public float DashCooldownCounter;
    public GameObject DashAnimation;
    private TokenManager _tokenManager;
    private MousePositionOverCollider _mouseOverCollider;



    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we are finding and assigning necessary scripts, Components and values and objects to variables.*/
    void Start()
    {
        ActiveMoveSpeed = MoveSpeed;
        PlayerAnimator = GetComponentInChildren<Animator>();
        _armPosition = new Vector2(ArmPivotGameObject.transform.position.x, ArmPivotGameObject.transform.position.y);
        _armPositionRunning = new Vector2(ArmPivotGameObject.transform.position.x + 1, ArmPivotGameObject.transform.position.y + 1);
        _grapplingHookController = GetComponent<GrapplingHookController>();
        DashAnimation.SetActive(false);
        CanWalk = true;
        _tokenManager = Object.FindObjectOfType<TokenManager>();
        _mouseOverCollider = Object.FindObjectOfType<MousePositionOverCollider>();
        DontDestroyOnLoad(gameObject);

    }

    ///Fixed Update is called based on a fixed frame rate.
    /**FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame. Compute Physics system calculations after FixedUpdate. 0.02 seconds (50 calls per second) is the default time between calls.*/
    /*! Note: This FixedUpdate fuction is fairly long, and more documentation will be written "inline" in the code itself.*/
    /*! The FixedUpdate function controls the overall movements that the player can do. This includes Walking/Running, Dashing, Jumping, Checking when to flip the player, and Animations related to this.*/
    async void FixedUpdate()
    {
        //Get the mouse position to screen position.
        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Find and assign the axis for walk movement.
        float x = Input.GetAxisRaw("Horizontal");
        //Only allow walking if player is not hooked with grapplinghook.
        if (_grapplingHookController.IsHooked == false)
        {
            if (CanWalk)
            {
                PlayerRigidbody.velocity = new Vector2(x * ActiveMoveSpeed, PlayerRigidbody.velocity.y);
            }
            else
            {
                PlayerRigidbody.velocity = new Vector2(x * 0, PlayerRigidbody.velocity.y);
            }
        }
        //Everything in this else scope will check for the following respectively: If player is not touching ground and is hooked give player a small boost boost as a Force.
        //If the player is touching ground and is hooked, the player can walk like normal.
        //Disable DashAnimation if the player is hooked and not touching ground and tries to move in opposite direction then what it is facing.
        //Stop all movement and dash animations whenever player stops pressing key.
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (IsTouchingGround == false)
                {
                    PlayerRigidbody.AddForce(Vector2.right * ActiveMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    PlayerRigidbody.velocity = new Vector2(x * ActiveMoveSpeed, PlayerRigidbody.velocity.y);
                }
                if (FacingRight && IsTouchingGround == false)
                {
                    DashAnimation.SetActive(true);
                }
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                PlayerRigidbody.velocity = new Vector2(x * 0, PlayerRigidbody.velocity.y);
                DashAnimation.SetActive(false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (IsTouchingGround == false)
                {
                    PlayerRigidbody.AddForce(Vector2.left * ActiveMoveSpeed * Time.deltaTime, ForceMode2D.Impulse);
                }
                else
                {
                    PlayerRigidbody.velocity = new Vector2(x * ActiveMoveSpeed, PlayerRigidbody.velocity.y);
                }
                if (!FacingRight && IsTouchingGround == false)
                {
                    DashAnimation.SetActive(true);
                }
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                PlayerRigidbody.velocity = new Vector2(x * 0, PlayerRigidbody.velocity.y);
                DashAnimation.SetActive(false);
            }
        }

        //Check if player is moving left, right, up or down.
        if (PlayerRigidbody.velocity.x > 0)
        {
            GoingRight = true;
            GoingLeft = false;
        }
        else if (PlayerRigidbody.velocity.x < 0)
        {
            GoingRight = false;
            GoingLeft = true;
        }
        else
        {
            GoingRight = false;
            GoingLeft = false;
        }

        //Run the actually calculations for the above checks.
        StartCoroutine(CalculateWhenMoving());

        //https://www.youtube.com/watch?v=xHargJbONls&ab_channel=Antarsoft
        //Set jump animations.
        PlayerAnimator.SetBool("Jump", !IsTouchingGround);
        PlayerAnimator.SetFloat("yVelocity", PlayerRigidbody.velocity.y);

        //Set Running animations, and change position of arm when running
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && IsTouchingGround == true && IsDashing == false)
        {
            PlayerAnimator.SetBool("Running", true);
            ArmPivotGameObject.transform.localPosition = new Vector2(-0.0625f, -0.0625f);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            ArmPivotGameObject.transform.localPosition = new Vector2(-0.125f, -0.09375f);
            PlayerAnimator.SetBool("Running", false);
        }

        //Check if the mouse position is on left side or right side, and then flip player accordingly.
        if (_mousePos.x < transform.position.x && FacingRight)
        {
            FlipPlayer();
        }
        else if (_mousePos.x > transform.position.x && !FacingRight)
        {
            FlipPlayer();
        }

        //Allow player to jump if player is not hooked, and is touching the ground.
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) && _grapplingHookController.IsHooked == false && IsCloseToRoof == false)
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
        {
            _tryingToJump = false;
        }
        //Allow player to increase the falling speed if key "S" is pressed.
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerRigidbody.gravityScale = 20;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            PlayerRigidbody.gravityScale = NormalGravity;

        }
        //Start Dash if Left Shift is pressed.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (DashCooldownCounter <= 0 && _tokenManager.WormTokenActive == false)
            {
                if (_tokenManager.RevloverTokenActive)
                {
                    _tokenManager.RevolverTokenDash();
                }
                StartCoroutine(Dash());
            }
            else if (_tokenManager.WormTokenActive && DashCooldownCounter <= 0)
            {
                var _insideCollider = _mouseOverCollider.CheckForCollider();
                if (_insideCollider == false)
                {
                    _tokenManager.WormTokenDash();
                    StartCoroutine(Dash());
                }
            }
        }
        //Check if player is dashing.
        //If true, set animation and arm position.
        if (IsDashing == true)
        {
            DashAnimation.SetActive(true);
            if (IsTouchingGround == true)
            {
                PlayerAnimator.SetBool("Running", true);
                ArmPivotGameObject.transform.localPosition = new Vector2(-0.0625f, -0.0625f);
            }
            else
            {
                ArmPivotGameObject.transform.localPosition = new Vector2(-0.125f, -0.09375f);
                PlayerAnimator.SetBool("Running", false);
            }
        }

        //Also checks if player is dashing.
        //Makes the player dash move towards the mouse position.
        if (IsDashing)
        {
            //////////////////////////////////////////////////////
            /*http://answers.unity.com/answers/633909/view.html */
            //////////////////////////////////////////////////////
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, ActiveMoveSpeed * Time.deltaTime);
        }
    }

    ///Flips the player
    /**Whenever this function is called the player will be flipped on the Y rotation axis. And the boolean FacingRight be set to the opposite of the current value.*/
    void FlipPlayer()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    /// IEnumerator for dashing that can be called with StartCoroutine().
    /** This IEnumerator allows to stop the process at any given moment, and then resume if wanted. All without stopping other processes or functions within the script.*/
    /*! The dash IEnumerator sets the gravity to zero, set the desired movement speed, cooldown time, and active time for the dash. Then it waits for the time it should be active.
    When the time is up, all variable changes goes back to "normal" and the cooldown timer starts. When that is done the cooldown counter is set back to 0, which allows the player to use dash again.*/
    IEnumerator Dash()
    {
        PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY; PlayerRigidbody.constraints = RigidbodyConstraints2D.None; PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerRigidbody.gravityScale = 0;
        DashCooldownCounter = DashCooldown;
        float tempspeed = MoveSpeed;
        ActiveMoveSpeed = DashSpeed;
        IsDashing = true;
        CanWalk = false;
        if (_tokenManager.CactusTokenActive) { StartCoroutine(_tokenManager.CactusTokenDash()); }
        yield return new WaitForSeconds(DashLength);
        ActiveMoveSpeed = tempspeed;
        CanWalk = true;
        PlayerRigidbody.gravityScale = NormalGravity;
        IsDashing = false;
        ArmPivotGameObject.transform.localPosition = new Vector2(-0.125f, -0.09375f);
        PlayerAnimator.SetBool("Running", false);
        DashAnimation.SetActive(false);
        yield return new WaitForSeconds(DashCooldown);
        DashCooldownCounter = 0;
    }

    ///Calculate when the player is moving to see which direction player is moving.
    /**Calculate based on an average moving model whenever the player is moving or not, and which directions the player is moving.*/
    /*!The IEnumerator check for movement change based on a 0.2 second intervals. If the new position after the interval on either X or Y axis is different the variables movement is set to true respectively.*/
    IEnumerator CalculateWhenMoving()
    {
        Vector3 prevPos = gameObject.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 actualPos = gameObject.transform.position;

        if (prevPos.y < actualPos.y && (actualPos.y - prevPos.y > 0.1))
        {
            GoingUp = true;
            GoingDown = false;
        }
        else if (prevPos.y > actualPos.y && (prevPos.y - actualPos.y > 0.1))
        {
            GoingUp = false;
            GoingDown = true;
        }
        else
        {
            GoingUp = false;
            GoingDown = false;
        }

        if (prevPos.x < actualPos.x && (actualPos.x - prevPos.x > 0.1))
        {
            GoingRight = true;
            GoingLeft = false;
        }
        else if (prevPos.x > actualPos.x && (prevPos.x - actualPos.x > 0.1))
        {
            GoingRight = false;
            GoingLeft = true;
        }
        else
        {
            GoingRight = false;
            GoingLeft = false;
        }

    }

    ///Allows the player to jump
    void Jump()
    {
        if (IsTouchingGround && CanWalk && IsCloseToRoof == false)
        {
            PlayerAnimator.SetBool("Jump", true);
            //Important, because when jumping while player already got an acting force in the Y-axis they will combine to give the player super jump. This prevents it.
            PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY; PlayerRigidbody.constraints = RigidbodyConstraints2D.None; PlayerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayerRigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Force);
        }
    }

}
