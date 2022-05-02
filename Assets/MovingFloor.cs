using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingFloor : MonoBehaviour
{
    private float _movedDistance;
    public float Speed;
    public bool Triggered = false;
    private bool _enabled = false;
    private Vector3 _initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Triggered) return;
        if (!_enabled)
        {
            GetComponent<TilemapRenderer>().enabled = true;
        }

        transform.position = new Vector3(transform.position.x - (Speed * Time.deltaTime), transform.position.y, transform.position.z);
        _movedDistance += Speed * Time.deltaTime;

        if (_movedDistance >= 2f)
        {
            transform.position = _initialPosition;
            _movedDistance = 0f;
        }
    }
}
