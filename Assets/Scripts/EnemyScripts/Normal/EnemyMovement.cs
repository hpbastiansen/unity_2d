using UnityEngine;

/// This script is responsible for all movement of the gunmen enemies. It does not make pathing decisions.
public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public bool IsTouchingGround = true;
    [HideInInspector] public bool IsCloseToRoof = false;
    [HideInInspector] public bool BlockedInFront = false;
    [HideInInspector] public bool GapInFront = false;

    private Rigidbody2D _rb;
    private Animator _enemyAnimator;
    private EnemyWeapon _enemyWeapon;
    
    [Header("Movement booleans")]
    public bool Running = false;
    [SerializeField] private float _runModifier = 2f;
    public bool MovingRight = false;
    public bool MovingLeft = false;
    public bool Jumping = false;
    public bool FacingRight = true;

    [Header("Speed and jump values")]
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _jumpForce = 50000f;

    /// Called before the first frame.
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponentInChildren<Animator>();
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<EnemyWeapon>();
    }

    /// Called every frame.
    /** The update function sets the enemy's direction. If the enemy has a target, flip the enemy to face it.
        Otherwise, flip the enemy so it's facing the direction of movement. 
        It also handles setting the running animation if they are moving or jumping. */
    private void Update()
    {
        if(_enemyWeapon.Target != null)
        {
            if (_enemyWeapon.Target.transform.position.x < transform.position.x && FacingRight) FlipEnemy();
            else if (_enemyWeapon.Target.transform.position.x > transform.position.x && !FacingRight) FlipEnemy();
        }
        else
        {
            if (_rb.velocity.x > 0 && !FacingRight) FlipEnemy();
            else if (_rb.velocity.x < 0 && FacingRight) FlipEnemy();
        }

        _enemyAnimator.SetBool("Running", MovingRight || MovingLeft || Jumping);
    }

    /// Called every physics cycle, 50 times every second.
    /** Calculates the movespeed depending on if the enemy is running or not. 
        Limit the velocity on the x-axis to the move speed. 
        Add a force in the set direction of movement. 
        If the enemy is jumping, set constraints to limit 'super jumping' and apply the jump force straight upwards. */
    void FixedUpdate()
    {
        float _moveSpeed = _speed * (Running ? _runModifier : 1);

        if (_rb.velocity.x > _moveSpeed) _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
        if (_rb.velocity.x < -_moveSpeed) _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);

        if (MovingRight)
        {
            _rb.AddForce(Vector3.right * _moveSpeed, ForceMode2D.Impulse);
        }

        if (MovingLeft)
        {
            _rb.AddForce(Vector3.left * _moveSpeed, ForceMode2D.Impulse);
        }

        if (Jumping)
        {
            if (IsTouchingGround && !IsCloseToRoof)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                _rb.constraints = RigidbodyConstraints2D.None;
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Force);
            }
        }
    }

    /// Flips the enemy 180 degrees on the Y-axis. Also flips the FacingRight boolean.
    private void FlipEnemy()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180f, 0);
    }
}
