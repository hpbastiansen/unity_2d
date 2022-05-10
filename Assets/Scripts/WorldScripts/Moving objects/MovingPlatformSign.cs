using UnityEngine;

/// This script is used to show the direction of travel on the signboards for moving platforms.
public class MovingPlatformSign : MonoBehaviour
{
    [SerializeField] private Transform _platform;
    private Animator _animator;
    private bool _goingLeft = false;
    private bool _goingRight = false;
    private float _oldPosX;

    /// Called before the first frame.
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _oldPosX = _platform.position.x;
    }

    /// Called every frame.
    /** Sets the going right and left variables according to the difference in position between frames and plays the correct animation. */
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
