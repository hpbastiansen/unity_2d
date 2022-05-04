using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [HideInInspector] public Vector3 InitialPosition;
    [HideInInspector] public Quaternion InitialRotation;
    [SerializeField] private bool _limitAngularVelocity;
    [SerializeField] private float _angularVelocityMax;
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        InitialPosition = gameObject.transform.position;
        InitialRotation = gameObject.transform.rotation;
    }

    private void Update()
    {
        if(_limitAngularVelocity)
        {
            if (_rb.angularVelocity > _angularVelocityMax) _rb.angularVelocity = _angularVelocityMax;
            if (_rb.angularVelocity < -_angularVelocityMax) _rb.angularVelocity = -_angularVelocityMax;
        }
    }
}
