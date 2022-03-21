///////////////////////////////////////
/*https://youtu.be/tMXgLBwtsvI?t=1984*/
///////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///The Parallax script allows any gameObject (often background or foreground elements) to move at different speed relative to the camera.
public class Parallax : MonoBehaviour
{
    public Camera MainCamera;
    Vector2 _startPosition;
    public float Strength;

    Vector2 _travelDistance => (Vector2)MainCamera.transform.position - _startPosition;


    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*!In the Start function we assign a starting position.*/
    public void Start()
    {
        _startPosition = transform.position;
    }

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*!Inside the Update funtion the postition of the gameObject is determined by it's start position, travel position and desired strength of the parallax.*/
    public void Update()
    {
        transform.position = _startPosition + _travelDistance * Strength;
    }
}
