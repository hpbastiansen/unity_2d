using UnityEngine;

/// This script makes the background appear to move infinitely behind the player, creating the illusion of speed.
public class MovingBackground : MonoBehaviour
{
    private float _movedDistance;
    public float Speed;
    public bool Triggered = false;
    private Vector3 _initialPosition;
    private bool _disabledParallax = false;

    /// Called before the first frame.
    void Start()
    {
        _movedDistance = 0f;
    }

    /// Called every frame.
    /** If the parallax script is enabled, it has to be turned off to be able to move the background from left to right. 
        If the background has been triggered, move it with a certain speed. 
        After moving 10 units, teleport it back to create the illusion of infinite movement using a small object. */
    void Update()
    {
        if (!Triggered) return;
        if(!_disabledParallax)
        {
            GetComponent<Parallax>().enabled = false;
            _disabledParallax = true;
            _initialPosition = transform.localPosition;
        }

        transform.localPosition = new Vector3(transform.localPosition.x - (Speed * Time.deltaTime), transform.localPosition.y, transform.localPosition.z);
        _movedDistance += Speed * Time.deltaTime;

        if(_movedDistance >= 10f)
        {
            transform.localPosition = _initialPosition;
            _movedDistance = 0f;
        }
    }
}
