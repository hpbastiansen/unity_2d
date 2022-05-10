using UnityEngine;
using UnityEngine.Tilemaps;

/// This script makes the floor appear to move under the player, creating the illusion of speed.
public class MovingFloor : MonoBehaviour
{
    private float _movedDistance;
    public float Speed;
    public bool Triggered = false;
    private bool _enabled = false;
    private Vector3 _initialPosition;

    /// Called before the first frame.
    /** Set the floors initial position. */
    void Start()
    {
        _initialPosition = transform.localPosition;
    }

    /// Called every frame.
    /** If the floor has been triggered, move the floor with a certain speed. 
        After moving 9.5 units, teleport it back to create the illusion of infinite movement using a small object. */
    void Update()
    {
        if (!Triggered) return;
        if (!_enabled)
        {
            GetComponent<TilemapRenderer>().enabled = true;
        }

        transform.localPosition = new Vector3(transform.localPosition.x - (Speed * Time.deltaTime), transform.localPosition.y, transform.localPosition.z);
        _movedDistance += Speed * Time.deltaTime;

        if (_movedDistance >= 9.5f)
        {
            transform.localPosition = _initialPosition;
            _movedDistance = 0f;
        }
    }
}
