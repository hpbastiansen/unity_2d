using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float Speed;
    private float _movedDistance = 0;
    private Vector3 _initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (Speed * Time.deltaTime), transform.position.y, transform.position.z);
        _movedDistance += Speed * Time.deltaTime;

        if (_movedDistance >= 15.625f)
        {
            transform.position = new Vector3(_initialPosition.x + (_movedDistance - 15.625f), _initialPosition.y, _initialPosition.z);
            _movedDistance = 0f;
        }
    }
}
