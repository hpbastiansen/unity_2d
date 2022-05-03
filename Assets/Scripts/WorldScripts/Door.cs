using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    enum Direction { Up, Down, Left, Right }

    [SerializeField] float _doorWidth;
    [SerializeField] float _doorHeight;
    [SerializeField] Direction _openDirection;
    [SerializeField] float _time;
    private Vector3 _velocity = Vector3.zero;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    private Transform _visualTransform;
    public bool Triggered = false;
    private Vector2 _openedPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _visualTransform = transform.Find("Visual");
        _visualTransform.position = new Vector3(_visualTransform.position.x - 0.5f + _doorWidth / 2, _visualTransform.position.y - 0.5f + _doorHeight / 2, _visualTransform.position.z);

        _collider = GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(_doorWidth, _doorHeight);
        _collider.offset = new Vector2(_doorWidth / 2, _doorHeight / 2);

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.size = new Vector2(_doorWidth, _doorHeight);

        if (_openDirection == Direction.Up)
        {
            _openedPosition = new Vector2(transform.position.x, transform.position.y + _doorHeight + 1);
        }
        else if (_openDirection == Direction.Down)
        {
            _openedPosition = new Vector2(transform.position.x, transform.position.y - _doorHeight - 1);
        }
        else if (_openDirection == Direction.Right)
        {
            _openedPosition = new Vector2(transform.position.x + _doorWidth + 1, transform.position.y);
        }
        else
        {
            _openedPosition = new Vector2(transform.position.x - _doorWidth - 1, transform.position.y);
        }
    }

    private void Update()
    {
        if (!Triggered) return;

        transform.position = Vector3.SmoothDamp(transform.position, _openedPosition, ref _velocity, _time);
    }
}
