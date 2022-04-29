using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Enemy components
    private Rigidbody2D _rb;
    private Animator _enemyAnimator;
    private Transform _firePoint;

    // Public booleans affecting movement.
    public bool Running = false;
    [SerializeField] private float _runModifier = 2f;
    public bool MovingRight = false;
    public bool MovingLeft = false;
    public bool Jumping = false;

    // Ground and roof check
    [HideInInspector] public bool IsTouchingGround = true;
    [HideInInspector] public bool IsCloseToRoof = false;

    // Speed and jump values
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _jumpForce = 50000f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponentInChildren<Animator>();
        _firePoint = transform.Find("Visuals/Arm/Weapon/FirePoint");
    }

    private void Update()
    {
        // Swap sprite and firing point to face direction of movement
        if (_rb.velocity.x > 0.01f)
        {
            _firePoint.eulerAngles = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_rb.velocity.x < -0.01f) 
        {
            _firePoint.eulerAngles = new Vector3(0, 0, 180);
            transform.localScale = new Vector3(-1, 1, 1); 
        }

        _enemyAnimator.SetBool("Running", MovingRight || MovingLeft || Jumping);
    }

    void FixedUpdate()
    {
        float _moveSpeed = _speed * (Running ? _runModifier : 1);

        if (_rb.velocity.x > _moveSpeed) _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
        if (_rb.velocity.x < -_moveSpeed) _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);

        if(MovingRight)
        {
            _rb.AddForce(Vector3.right * _moveSpeed, ForceMode2D.Impulse);
        }
        if(MovingLeft)
        {
            _rb.AddForce(Vector3.left * _moveSpeed, ForceMode2D.Impulse);
        }
        if(Jumping)
        {
            if(IsTouchingGround && !IsCloseToRoof)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                _rb.constraints = RigidbodyConstraints2D.None;
                _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                _rb.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Force);
            }
        }
    }


}
