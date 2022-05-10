using UnityEngine;

/// This script manages a modular door system. A width, height, opening direction and time can be specified.
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

    /// Called before the first frame.
    /** First, we set the position of the door visuals. This has to adjust for the height and width specified. 
        Afterwards, we have to set the collider size and offset so it matches the door's width and height.
        Then, we set the size of the sprite to match the collider size.
        Lastly, we set the point the door should travel towards according to the opening direction set. */
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

    /// Called every frame.
    /** If the door has been triggered, we move it towards the openedPosition point using the smoothdamp method, 
        making it accelerate and decelerate smoothly. */
    private void Update()
    {
        if (!Triggered) return;

        transform.position = Vector3.SmoothDamp(transform.position, _openedPosition, ref _velocity, _time);
    }
}
