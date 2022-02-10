//https://github.com/Brackeys/Smooth-Camera-Follow/blob/master/Smooth%20Camera/Assets/CameraFollow.cs
using UnityEngine;
public class CameraFollower : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.Rotate(10, 10, 10);
        transform.LookAt(target);
    }

}