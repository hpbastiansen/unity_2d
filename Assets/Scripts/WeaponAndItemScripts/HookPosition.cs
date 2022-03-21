using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///The HookPosition script tells the visual hook object how to behave.
public class HookPosition : MonoBehaviour
{
    public GameObject FirePoint;
    public GameObject Arm;
    public Movement PlayerMovement;


    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/

    /*! In this Update function the visual hook object is set to always follow a spesific position on the gun when the player is aiming in different directions.
    It is also set to always be facing towards downwards to simulate/visualize the gravity effect which it would have in real life.*/
    void Update()
    {
        if (PlayerMovement.FacingRight == true)
        {
            var _armRotation = Arm.transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(0, 0, _armRotation * -1);
            transform.position = FirePoint.transform.position;
        }
        else
        {
            var _armRotation = Arm.transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(0, 0, _armRotation * -1);
            transform.position = FirePoint.transform.position;
        }
    }
}
