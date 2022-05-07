using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private float _losConeWidth;
    private List<GameObject> _grapplingPoints = new List<GameObject>();
    [HideInInspector] public GameObject ClosestGrapplingPoint = null;
    private PolygonCollider2D _losCollider;
    private GameObject _player;
    private GrapplingHookController _grapplingHookController;

    private void Awake()
    {
        _losCollider = GetComponent<PolygonCollider2D>();
        _player = transform.root.gameObject;
        _grapplingHookController = _player.GetComponent<GrapplingHookController>();
        SetLOSCollider();
    }

    // Update is called once per frame
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

    private void SetLOSCollider()
    {
        Vector2 _pointA = _player.transform.position;
        Vector2 _pointB = new Vector2(_player.transform.position.x + _grapplingHookController.MaxDistance, _player.transform.position.y + _losConeWidth);
        Vector2 _pointC = new Vector2(_player.transform.position.x + _grapplingHookController.MaxDistance, _player.transform.position.y - _losConeWidth);

        _losCollider.SetPath(0, new Vector2[] { _pointA, _pointB, _pointC });
    }

    private void RotateToCursor()
    {
        Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _mousePosition - transform.position) * Quaternion.Euler(0, 0, 90);
    }

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
