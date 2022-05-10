using UnityEngine;

/// This script creates a simple path between two points for a worm enemy to follow.
public class WormPathSimple : MonoBehaviour
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private bool _pathSet = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _endDistance;

    /// Called before the first frame.
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// Called from a worm spawner object to start moving the worm enemy. Sets the direction of travel.
    public void Init()
    {
        _direction = (EndPoint - StartPoint).normalized;
        _pathSet = true;
    }

    /// Called every physics cycle, 50 times every second.
    /** If the path is set, calculate the angle using the distance, then move towards that direction. 
        If the worm is close to its end distance, invoke the method to delete itself after 2 seconds have passed. */
    private void FixedUpdate()
    {
        if (!_pathSet) return;

        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + 270;
        transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, EndPoint, _speed * Time.deltaTime), Quaternion.AngleAxis(_angle, Vector3.forward));

        float _distance = Vector2.Distance(_rb.position, EndPoint);
        if (_distance < _endDistance)
        {
            _pathSet = false;
            Invoke("DeleteSelf", 2f);
        }
    }

    /// Delete this gameobject.
    private void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
