using System.Collections.Generic;
using UnityEngine;

/// This script sets the worm's polygon collider. It is called every frame from the WormBody script.
public class WormCollision : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _force;
    private PolygonCollider2D _polygonCollider;
    private RectTransform _wormHead;

    /// Called before the first frame.
    private void Start()
    {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _wormHead = GetComponentInChildren<RectTransform>();
    }

    /// Set up the Polygon Collider using the positions of the body segments. Called from WormBody script.
    public void SetCollision(Vector3[] _segmentPoses, int _length, float _width)
    {
        _polygonCollider.pathCount = _length;

        // Get corners of worm head and set first path of polygon collider.
        Vector3[] _headCornersV3 = new Vector3[4];
        Vector2[] _headCorners = new Vector2[4];
        _wormHead.GetLocalCorners(_headCornersV3);
        for (int _i = 0; _i < 4; _i++)
        {
            _headCorners[_i] = _headCornersV3[_i];
        }
        _polygonCollider.SetPath(0, _headCorners);

        // Set polygon collider for each body segment.
        for (int _i = 0; _i < _length-1; _i++)
        {
            List<Vector2> _currentPositions = new List<Vector2>
            {
                _segmentPoses[_i],
                _segmentPoses[_i+1]
            };

            List<Vector2> _currentColliderPoints = CalculateColliderPoints(_currentPositions, _width);
            _polygonCollider.SetPath(_i+1, _currentColliderPoints.ConvertAll(_p => (Vector2)transform.InverseTransformPoint(_p)));
        }
    }

    /// Calculates the points for the Polygon Collider given the start and end of the body segment.
    /** Given a line and the line renderer's width, calculates the bounding rectangle for the collider. */
    private List<Vector2> CalculateColliderPoints(List<Vector2> _positions, float _width)
    {

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

    /// Called on a collider entering the trigger on the gameobject.
    /** If the player touches the worm, damage the player and knock them back in the opposite direction.
        If the player is hooked, release the grappling hook. */
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            float _deltaX = transform.position.x - _other.transform.position.x;
            _other.GetComponent<PlayerHealth>().TakeDamage(_damage, _force, _deltaX > 0 ? 135f : 45f);
            if(_other.GetComponent<GrapplingHookController>().IsHooked)
            {
                _other.GetComponent<GrapplingHookController>().ReleaseGrapple();
            }
        }
    }
}
