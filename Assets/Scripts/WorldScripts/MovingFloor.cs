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
        _initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Triggered) return;
        if (!_enabled)
        {
            GetComponent<TilemapRenderer>().enabled = true;
        }

        transform.localPosition = new Vector3(transform.localPosition.x - (Speed * Time.deltaTime), transform.localPosition.y, transform.localPosition.z);
        _movedDistance += Speed * Time.deltaTime;

        if (_movedDistance >= 9.5f)
        {
            transform.localPosition = _initialPosition;
            _movedDistance = 0f;
        }
    }
}
