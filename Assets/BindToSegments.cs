using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindToSegments : MonoBehaviour
{
    [SerializeField] private GameObject _segments;
    private float _distanceToSegments;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_segments.transform.localPosition.x + _distanceToSegments, transform.position.y, transform.position.z);
    }
}
