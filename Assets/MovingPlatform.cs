using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_pointA, 1f);
        Gizmos.DrawWireSphere(_pointB, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_pointA, _pointB);
    }

    // Start is called before the first frame update
    void Start()
    {
        _pointA = transform.position;
        _pointB = (Vector2)transform.position + new Vector2(_moveX, _moveY);
        if(_offset > 0)
        {
            _stopped = true;
            _goingToB = false;
            Invoke("StartMoving", _offset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopped) return;
        if(_goingToB)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _pointB, ref _velocity, _time);
            if (Vector3.Distance(transform.position, _pointB) < 0.2f)
            {
                _stopped = true;
                Invoke("StartMoving", 1f);
            }
        } else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _pointA, ref _velocity, _time);
            if (Vector3.Distance(transform.position, _pointA) < 0.2f)
            {
                _stopped = true;
                Invoke("StartMoving", 1f);
            }
        }
    }

    private void StartMoving()
    {
        _goingToB = !_goingToB;
        _stopped = false;
    }
}
