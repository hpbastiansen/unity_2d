//https://github.com/Brackeys/Smooth-Camera-Follow/blob/master/Smooth%20Camera/Assets/CameraFollow.cs
using UnityEngine;
public class CameraFollower : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.02f;
    public float offsetspeed;
    public float offsetX;
    public float offsetY;

    public Vector3 offset;
    public Vector3 newoffset;

    public Movement mv;

    void Start()
    {
        mv = GameObject.Find("Main_Character").GetComponent<Movement>();
        offset = new Vector3(0, .83f, -300);
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}