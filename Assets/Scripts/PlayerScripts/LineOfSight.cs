using System.Collections.Generic;
using UnityEngine;

/// This script creates a Line of Sight collider on the player rotated towards the mouse cursor, and finds the closest grappling hook point inside it.
public class LineOfSight : MonoBehaviour
{
    [SerializeField] private float _losConeWidth;
    private List<GameObject> _grapplingPoints = new List<GameObject>();
    [HideInInspector] public GameObject ClosestGrapplingPoint = null;
    private PolygonCollider2D _losCollider;
    private GameObject _player;
    private GrapplingHookController _grapplingHookController;

    /// Called at initialization, before all objects Start() methods.
    /** Set the collider's points at startup. */
    private void Awake()
    {
        _losCollider = GetComponent<PolygonCollider2D>();
        _player = transform.root.gameObject;
        _grapplingHookController = _player.GetComponent<GrapplingHookController>();
        SetLOSCollider();
    }

    /// Called every frame.
    /** Rotate the collider towards the mouse. 
        If the player is not currently grappled to the closest grappling point, show a targeter on it. */
    void Update()
    {
        RotateToCursor();

        if (ClosestGrapplingPoint)
        {
            if (_grapplingHookController.IsHooked && _grapplingHookController.HookedPoint == ClosestGrapplingPoint)
            {
                ClosestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                ClosestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    /// This sets the polygon collider attached to the gameobject. The collider is shaped like a triangle with the point at the player's position.
    private void SetLOSCollider()
    {
        Vector2 _pointA = _player.transform.position;
        Vector2 _pointB = new Vector2(_player.transform.position.x + _grapplingHookController.MaxDistance, _player.transform.position.y + _losConeWidth);
        Vector2 _pointC = new Vector2(_player.transform.position.x + _grapplingHookController.MaxDistance, _player.transform.position.y - _losConeWidth);

        _losCollider.SetPath(0, new Vector2[] { _pointA, _pointB, _pointC });
    }

    /// Gets the world coordinates of the mouse cursor's position, and rotates the collider towards it.
    private void RotateToCursor()
    {
        Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _mousePosition - transform.position) * Quaternion.Euler(0, 0, 90);
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** If the collision is with a 'grappleable object', add it to the list of grappling points in range. 
        Then check if this new point is closer than any of the other points currently in range. 
        If it is, disable the targeter on the previous closest point and show it on the new one. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GRAPPABLEOBJECTS"))
        {
            if (!_grapplingPoints.Contains(collision.gameObject))
            {
                _grapplingPoints.Add(collision.gameObject);

                if (!ClosestGrapplingPoint)
                {
                    ClosestGrapplingPoint = collision.gameObject;
                }
                else
                {
                    if (Vector3.Distance(_player.transform.position, ClosestGrapplingPoint.transform.position) > Vector3.Distance(_player.transform.position, collision.transform.position))
                    {
                        ClosestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = false;
                        ClosestGrapplingPoint = collision.gameObject;
                    }
                }
            }
        }
    }

    /// Called on a collider exiting the trigger on the gameobject.
    /** If the object exiting the trigger is a 'grappleable object', remove it from the list of grappling points in range.
        If this object was the closest grappling point, find a new closest point. If there are no points in range, set it to 'null'. */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("GRAPPABLEOBJECTS"))
        {
            if (_grapplingPoints.Contains(collision.gameObject))
            {
                _grapplingPoints.Remove(collision.gameObject);
            }

            if (collision.gameObject == ClosestGrapplingPoint)
            {
                ClosestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = false;
                if (_grapplingPoints.Count > 0)
                {
                    GameObject _closest = null;
                    foreach (GameObject _grapplingPoint in _grapplingPoints)
                    {
                        if (!_closest) _closest = _grapplingPoint;
                        if (Vector3.Distance(_player.transform.position, _closest.transform.position) > Vector3.Distance(_player.transform.position, _grapplingPoint.transform.position))
                        {
                            _closest = _grapplingPoint;
                        }
                    }
                    ClosestGrapplingPoint = _closest;
                }
                else
                {
                    ClosestGrapplingPoint = null;
                }
            }
        }
    }

}
