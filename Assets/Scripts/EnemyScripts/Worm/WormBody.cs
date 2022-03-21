// https://www.youtube.com/watch?v=9hTnlp9_wX8 Creating the worm body
// https://www.youtube.com/watch?v=BfP0KyOxVWs Collision for line renderer

using System.Collections.Generic;
using UnityEngine;

///The WormBody script handles the drawing and collision of the worm enemy's body.

public class WormBody : MonoBehaviour
{
    [Header("Body Length")]
    [SerializeField] private int _minLength = 4;
    [SerializeField] private int _maxLength = 8;
    private int _length;
    
    [Header("Body Segments")]
    [SerializeField] private Transform _targetDir;
    [SerializeField] private float _targetDist;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private float _trailSpeed;

    [Header("Wiggle")]
    [SerializeField] private Transform _wiggleDir;
    [SerializeField] private float _wiggleSpeed;
    [SerializeField] private float _wiggleMagnitude;

    private Vector3[] _segmentPoses;
    private Vector3[] _segmentV;

    private LineRenderer _lineRend;
    private PolygonCollider2D _polygonCollider;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we find the necessary components, choose the worm's length and prepare the Line Renderer.*/
    private void Start()
    {
        _lineRend = GetComponent<LineRenderer>();
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _length = (int)Mathf.Floor(Random.Range(_minLength, _maxLength));
        _lineRend.positionCount = _length;
        _segmentPoses = new Vector3[_length];
        _segmentV = new Vector3[_length];
        ResetPos();
    }

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
     * This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! Note: This update fuction is fairly long, and more documentation will be written "inline" in the code itself.*/
    /*! The update function moves the body segments sequentially, creating a natural flowing movement. We also draw the Polygon Collider around the segments. */
    private void Update()
    {
        // Changes the wiggle rotation based on the current time passed into a sine function.
        _wiggleDir.localRotation = Quaternion.Euler(0, 0, (Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleMagnitude) + 270);

        // Sets the position of the first body segment manually.
        _segmentPoses[0] = _targetDir.position;

        // Moves the other body segments smoothly behind the first one.
        for(int _i = 1; _i < _segmentPoses.Length; _i++)
        {
            _segmentPoses[_i] = Vector3.SmoothDamp(_segmentPoses[_i], _segmentPoses[_i - 1] + _targetDir.right * _targetDist, ref _segmentV[_i], _smoothSpeed + _i / _trailSpeed);
            //Vector3 _targetPos = _segmentPoses[_i - 1] + (_segmentPoses[_i] - _segmentPoses[_i - 1]).normalized * _targetDist;
            //_segmentPoses[_i] = Vector3.SmoothDamp(_segmentPoses[_i], _targetPos, ref _segmentV[_i], _smoothSpeed);
        }
        _lineRend.SetPositions(_segmentPoses);

        // Set up the Polygon Collider with the positions of the body segments.
        _polygonCollider.pathCount = _length-1;
        for(int _i = 0; _i < _length-1; _i++)
        {
            List<Vector2> _currentPositions = new List<Vector2>
            {
                _segmentPoses[_i],
                _segmentPoses[_i+1]
            };

            List<Vector2> _currentColliderPoints = CalculateColliderPoints(_currentPositions);
            _polygonCollider.SetPath(_i, _currentColliderPoints.ConvertAll(_p => (Vector2)transform.InverseTransformPoint(_p)));
        }
    }

    ///Resets the positions of the Line Renderer so they do not start at (0, 0, 0) in world space. Used only at instantiation.
    private void ResetPos()
    {
        _segmentPoses[0] = _targetDir.position;
        for(int _i = 1; _i < _length; _i++)
        {
            _segmentPoses[_i] = _segmentPoses[_i - 1] + _targetDir.right * _targetDist;
        }
        _lineRend.SetPositions(_segmentPoses);
    }

    ///Calculates the points for the Polygon Collider given the start and end of the body segment.
    /** Given a line and the line renderer's width, calculates the bounding rectangle for the collider. */
    private List<Vector2> CalculateColliderPoints(List<Vector2> _positions)
    {
        float _width = _lineRend.startWidth;

        float _m = (_positions[1].y - _positions[0].y) / (_positions[1].x - _positions[0].x);
        float _deltaX = (_width / 2f) * (_m / Mathf.Pow(_m * _m + 1, 0.5f));
        float _deltaY = (_width / 2f) * (1 / Mathf.Pow(1 + _m * _m, 0.5f));

        Vector2[] _offsets = new Vector2[2];
        _offsets[0] = new Vector2(-_deltaX, _deltaY);
        _offsets[1] = new Vector2(_deltaX, -_deltaY);

        List<Vector2> _colliderPositions = new List<Vector2>
        {
            _positions[0] + _offsets[0],
            _positions[1] + _offsets[0],
            _positions[1] + _offsets[1],
            _positions[0] + _offsets[1]
        };

        return _colliderPositions;
    }
}
