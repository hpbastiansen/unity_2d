using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform subject;
    Vector2 startPosition;
    float startZ;
    public float strength;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    Vector2 parallaxFactor;

    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }
    public void Update()
    {
        transform.position = startPosition + travel * strength;
    }
}
