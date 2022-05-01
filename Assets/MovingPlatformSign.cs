using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSign : MonoBehaviour
{
    [SerializeField] private Transform _platform;
    private Animator _animator;
    private bool _goingLeft = false;
    private bool _goingRight = false;
    private float _oldPosX;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _oldPosX = _platform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float _xDiff = _platform.position.x - _oldPosX;

        if(_xDiff > 0)
        {
            _goingLeft = false;
            _goingRight = true;
        } 
        else if(_xDiff < 0)
        {
            _goingRight = false;
            _goingLeft = true;
        }
        else
        {
            _goingRight = false;
            _goingLeft = false;
        }

        _oldPosX = _platform.position.x;

        _animator.SetBool("goingLeft", _goingLeft);
        _animator.SetBool("goingRight", _goingRight);
    }
}
