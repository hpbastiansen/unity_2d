using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMovement : MonoBehaviour
{
    [SerializeField] private List<GameObject> _segments;
    [SerializeField] private float _speed;
    private float _segmentWidth;

    // Start is called before the first frame update
    void Start()
    {
        _segmentWidth = _segments[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject _segment in _segments)
        {
            Vector3 _newPosition = new Vector3(_segment.transform.position.x + (_speed * Time.deltaTime), _segment.transform.position.y, _segment.transform.position.z);
            _segment.transform.position = _newPosition;
        }

        
    }
}
