using UnityEngine;
using System.Collections;

/// This script makes a certain gameobject move from the start point to the end point over a certain time.
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveY;
    [SerializeField] private float _time;
    [SerializeField] private float _offset;

    private Vector2 _pointA;
    private Vector2 _pointB;

    private Vector3 _velocity = Vector3.zero;

    private bool _goingToB = true;
    private bool _stopped = false;

    /// Draw the start and end points and a line between them in the editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_pointA, 1f);
        Gizmos.DrawWireSphere(_pointB, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_pointA, _pointB);
    }

    /// Called before the first frame.
    /** Calculate the two points to move between, and if an offset is specified, don't move until that time has passed. */
    void Start()
    {
        _pointA = transform.position;
        _pointB = (Vector2)transform.position + new Vector2(_moveX, _moveY);
        if(_offset > 0)
        {
            _stopped = true;
            _goingToB = false;
            StartCoroutine(StartMovingAfterSeconds(_offset));
        }
    }

    /// Called every frame.
    /** Move towards the current point using the smoothdamp method, starting and stopping smoothly. 
        If within 0.2 units of the point, stop moving and wait for 1 second before switching direction. */
    void Update()
    {
        if (_stopped) return;
        if(_goingToB)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _pointB, ref _velocity, _time);
            if (Vector3.Distance(transform.position, _pointB) < 0.2f)
            {
                _stopped = true;
                StartCoroutine(StartMovingAfterSeconds(1f));
            }
        } 
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _pointA, ref _velocity, _time);
            if (Vector3.Distance(transform.position, _pointA) < 0.2f)
            {
                _stopped = true;
                StartCoroutine(StartMovingAfterSeconds(1f));
            }
        }
    }

    /// Switch direction of movement and start moving after waiting for an amount of seconds.
    IEnumerator StartMovingAfterSeconds(float _time)
    {
        yield return new WaitForSeconds(_time);
        _goingToB = !_goingToB;
        _stopped = false;
    }
}
