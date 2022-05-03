using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPathSimple : MonoBehaviour
{
    [HideInInspector] public Vector2 StartPoint;
    [HideInInspector] public Vector2 EndPoint;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private bool _pathSet = false;
    [SerializeField] private float _speed;
    [SerializeField] private float _endDistance;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        _direction = (EndPoint - StartPoint).normalized;
        _pathSet = true;
    }

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

    private void DeleteSelf()
    {
        Destroy(gameObject);
    }
}
