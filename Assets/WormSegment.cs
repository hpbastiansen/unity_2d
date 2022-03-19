// https://www.youtube.com/watch?v=sPlcecIh3ik

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSegment : MonoBehaviour
{
    public class Marker
    {
        public Vector2 position;
        public Quaternion rotation;

        public Marker(Vector2 position_, Quaternion rotation_)
        {
            position = position_;
            rotation = rotation_;
        }
    }

    public List<Marker> markerList = new List<Marker>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMarkerList();
    }

    public void UpdateMarkerList()
    {
        markerList.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList()
    {
        markerList.Clear();
        markerList.Add(new Marker(transform.position, transform.rotation));
    }
}
