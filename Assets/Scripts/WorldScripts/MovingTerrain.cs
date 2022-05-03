using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTerrain : MonoBehaviour
{
    public bool Triggered = false;

    [SerializeField] private float _moveX;
    [SerializeField] private float _moveY;
    [SerializeField] private float _speed;

    private Vector2 _pointFrom;
    private Vector2 _pointTo;

    // Start is called before the first frame update
    void Start()
    {
        _pointFrom = transform.position;
        _pointTo = (Vector2)transform.position + new Vector2(_moveX, _moveY);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Triggered) return;

        transform.position = Vector3.MoveTowards(transform.position, _pointTo, _speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_pointFrom, 1f);
        Gizmos.DrawWireSphere(_pointTo, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_pointFrom, _pointTo);
    }
}
