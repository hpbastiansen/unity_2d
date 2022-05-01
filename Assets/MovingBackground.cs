using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    private float _movedDistance;
    [SerializeField] private float _speed;
    public bool Triggered = false;
    private Vector3 _initialPosition;
    private bool _disabledParallax = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _movedDistance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Triggered) return;
        if(!_disabledParallax)
        {
            GetComponent<Parallax>().enabled = false;
            _disabledParallax = true;
            _initialPosition = transform.position;
        }

        transform.position = new Vector3(transform.position.x + (_speed * Time.deltaTime), transform.position.y, transform.position.z);
        _movedDistance += _speed * Time.deltaTime;

        if(_movedDistance >= 10f)
        {
            transform.position = _initialPosition;
            _movedDistance = 0f;
        }
    }
}
