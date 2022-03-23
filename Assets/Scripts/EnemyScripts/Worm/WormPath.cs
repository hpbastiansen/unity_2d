// http://answers.unity.com/answers/1700931/view.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///The WormPath script paths a quadratic bezier curve for the worm enemy to follow.

public class WormPath : MonoBehaviour
{
    [HideInInspector] public Vector2 StartPoint;
    [HideInInspector] public Vector2 PlayerPoint;
    [HideInInspector] public Vector2 EndPoint;
    private Vector2 _bestMiddlePoint;

    [SerializeField] private int _pathSamples = 20;
    private List<Vector2> _points;
    private int _currentPoint = 0;
    private bool _pathSet = false;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _nextPointDistance = 1f;

    [Header("Gizmos")]
    [SerializeField] private bool _drawPath = true;
    [SerializeField] private bool _drawControlPoints = true;

    private Rigidbody2D _rb;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we get the Rigidbody2D component of the worm.*/
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        if(_drawPath)
        {
            Gizmos.color = Color.green;
            foreach (Vector2 _point in _points)
            {
                Gizmos.DrawWireSphere(_point, 0.1f);
            }
        }
        if(_drawControlPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(StartPoint, 0.2f);
            Gizmos.DrawSphere(_bestMiddlePoint, 0.2f);
            Gizmos.DrawSphere(EndPoint, 0.2f);
        }
    }

    ///The Init method sets the worm's path.
    /**This method is called by the Worm Spawner gameobject. Once path has been found the worm can start moving through it. */
    public void Init()
    {
        _bestMiddlePoint = CalculateBestMiddlePoint(StartPoint, EndPoint, PlayerPoint);
        _points = CalculateRoute();
        _pathSet = true;
    }

    ///FixedUpdate is called every physics step.
    /**The FixedUpdate function is non-FPS dependent, and is executed exactly in sync with the physics engine.*/
    /*! The FixedUpdate function moves and rotates the worm along the path set. When the worm is close to the current point, it changes to the next in the path. */
    private void FixedUpdate()
    {
        if (!_pathSet) return;

        // Find direction to next waypoint, and rotate and move towards it.
        Vector3 _direction = (_points[_currentPoint] - _rb.position).normalized;
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 270;
        transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, _points[_currentPoint], _speed * Time.deltaTime), Quaternion.AngleAxis(_angle, Vector3.forward));

        // Check if we are close enough to the current point, and if so, select the next one. Mark gameobject for deletion if the path is complete.
        float _distance = Vector2.Distance(_rb.position, _points[_currentPoint]);
        if(_distance < _nextPointDistance)
        {
            _currentPoint++;
            if (_currentPoint >= _points.Count)
            {
                _pathSet = false;
                StartCoroutine(MarkForDeletion());
            }
        }
    }

    
    /// Given a quadratic bezier curve running from aStart to aEnd through aPoint, finds the control point when the curve runs through aPoint at aTime.
    private Vector2 CalculateMiddlePoint(Vector2 _aStart, Vector2 _aEnd, float _aTime, Vector2 _aPoint)
    {
        float _t = _aTime;
        float _rt = 1f - _t;
        return 0.5f * (_aPoint - _rt * _rt * _aStart - _t * _t * _aEnd) / (_t * _rt);
    }

    /// Given a quadratic bezier curve running from aStart to aEnd through aPoint, finds the best fitting 't' value of aPoint using the distances between the three points.
    private float CalculateMiddleTime(Vector2 _aStart, Vector2 _aEnd, Vector2 _aPoint)
    {
        float _a = Vector2.Distance(_aPoint, _aStart);
        float _b = Vector2.Distance(_aPoint, _aEnd);
        return _a / (_a + _b);
    }

    /// Given a quadratic bezier curve running from aStart to aEnd through aPoint, finds the best fitting control point using the distances between the three points.
    private Vector2 CalculateBestMiddlePoint(Vector2 _aStart, Vector2 _aEnd, Vector2 _aPoint)
    {
        return CalculateMiddlePoint(_aStart, _aEnd, CalculateMiddleTime(_aStart, _aEnd, _aPoint), _aPoint);
    }

    /// Returns a point on a quadratic bezier curve (p0, p1, p2) given t between 0 and 1.
    private Vector2 CalculateQuadraticBezierPoint(float _t, Vector2 _p0, Vector2 _p1, Vector2 _p2)
    {
        float _u = 1 - _t;
        float _tt = _t * _t;
        float _uu = _u * _u;
        Vector2 _p = _uu * _p0;
        _p += 2 * _u * _t * _p1;
        _p += _tt * _p2;
        return _p;
    }

    /// Returns an amount of points along a quadratic bezier curve.
    private List<Vector2> CalculateRoute()
    {
        List<Vector2> _route = new List<Vector2>();
        for(int _i = 0; _i < _pathSamples-1; _i++)
        {
            float _t = (float)_i / _pathSamples;
            _route.Add(CalculateQuadraticBezierPoint(_t, StartPoint, _bestMiddlePoint, EndPoint));
        }
        _route.Add(CalculateQuadraticBezierPoint(1, StartPoint, _bestMiddlePoint, EndPoint));
        return _route;
    }

    ///Destroy the worm object after 3 seconds of reaching path. This allows the worms body to get into the ground before deletion.
    private IEnumerator MarkForDeletion()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
