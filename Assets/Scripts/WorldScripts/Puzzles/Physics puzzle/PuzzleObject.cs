using UnityEngine;

/// This script is used on objects belonging to a physics puzzle. It stores the initial position and rotation of the object.
public class PuzzleObject : MonoBehaviour
{
    [HideInInspector] public Vector3 InitialPosition;
    [HideInInspector] public Quaternion InitialRotation;
    [SerializeField] private bool _limitAngularVelocity;
    [SerializeField] private float _angularVelocityMax;
    private Rigidbody2D _rb;

    /// Called before the first frame.
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        InitialPosition = gameObject.transform.position;
        InitialRotation = gameObject.transform.rotation;
    }

    /// If the object is set to limit angular velocity and the angular velocity exceeds the max set, reset it back to the max.
    private void Update()
    {
        if(_limitAngularVelocity)
        {
            if (_rb.angularVelocity > _angularVelocityMax) _rb.angularVelocity = _angularVelocityMax;
            if (_rb.angularVelocity < -_angularVelocityMax) _rb.angularVelocity = -_angularVelocityMax;
        }
    }
}
