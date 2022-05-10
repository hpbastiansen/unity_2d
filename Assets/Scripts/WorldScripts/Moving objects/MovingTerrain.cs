using UnityEngine;

/// This script makes a gameobject move a specified X and Y distance in the world at a specific speed.
public class MovingTerrain : MonoBehaviour
{
    public bool Triggered = false;

    [SerializeField] private float _moveX;
    [SerializeField] private float _moveY;
    [SerializeField] private float _speed;

    private Vector2 _pointFrom;
    private Vector2 _pointTo;

    /// Called before the first frame.
    /** Calculate the starting and ending points from the specified parameters. */
    void Start()
    {
        _pointFrom = transform.position;
        _pointTo = (Vector2)transform.position + new Vector2(_moveX, _moveY);
    }

    /// Called every frame.
    /** If the terrain has been triggered, move it towards the ending point with the speed specified. */
    void Update()
    {
        if (!Triggered) return;
        transform.position = Vector3.MoveTowards(transform.position, _pointTo, _speed * Time.deltaTime);
    }

    /// This method draws the starting point, ending point and a line between them in the Unity editor.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_pointFrom, 1f);
        Gizmos.DrawWireSphere(_pointTo, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_pointFrom, _pointTo);
    }
}
