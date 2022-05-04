using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///The ArmPivot script allows the arm of the player to be rotated to always face the mouse position.
public class ArmPivot : MonoBehaviour
{
    public GameObject MyPlayer;
    private Vector3 _mousePosition;
    public Movement PlayerMovementScript;
    public Transform ObjectToRotate;
    private Vector3 _objectPosition;
    private float _angle;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/

    /*! In the Start function the player Movement script is located*/
    public void Start()
    {
        PlayerMovementScript = GameObject.Find("Main_Character").GetComponent<Movement>();
    }

    ///Update is called every frame
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often*/

    /*! In the Update function we are checking for the mouse position in the world and converting it to its on screen position. 
    We are then finding the angle in degrees based on the Y and X postion of the mouse, and storing it in the _angle variable.
    Lastly we give the gameObject carrying the script the same rotation as the angle.
    We also check if the player is flipped or not. If the player is flipped (See FlipPlayer() function in Movement script) we have to flip the object aswell and use the negative value of the angle*/
    public void Update()
    {
        if (PlayerMovementScript.NoControl) return;
        //http://answers.unity.com/answers/130142/view.html
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 5.23f; //The distance between the camera and object
        _objectPosition = Camera.main.WorldToScreenPoint(ObjectToRotate.position);
        _mousePosition.x = _mousePosition.x - _objectPosition.x;
        _mousePosition.y = _mousePosition.y - _objectPosition.y;
        _angle = Mathf.Atan2(_mousePosition.y, _mousePosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));

        if (!PlayerMovementScript.FacingRight)
        {
            transform.rotation = Quaternion.Euler(-180, 0, -_angle);
        }
    }

}
