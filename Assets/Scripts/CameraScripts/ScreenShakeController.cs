using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=8PXPyyVu_6I&t=49s&ab_channel=gamesplusjames

///The ScreenShakeController allows any script to start a screen shake effect.
public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance;
    private float _shakeTimeRemaining, _shakePower, _shakeFadeTime, _shakeRotation;
    public float RotationMultiplier = 15f;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function the Instance variable is set to a copy of the script itself which is applied to the camera object*/
    void Start()
    {
        Instance = this;
    }

    /// LateUpdate is called every frame
    /** LateUpdate is called after all Update functions have been called. This is useful to order script execution. 
    The camera follow is implemented in LateUpdate because it tracks objects that might have moved inside Update.*/
    /*! In the Late Update the script checks if the shake time remaining is bigger then zero. If it is that means it should shake. Inside the if statement it is constantly given
    random x and y positions and rotations that is multiplied with the shake power, which we then use to move the camera to its locations. Thus giving it a randomized shake feeling.*/
    private void LateUpdate()
    {
        if (_shakeTimeRemaining > 0)
        {
            _shakeTimeRemaining -= Time.deltaTime;
            float _xAmount = Random.Range(-1f, 1f) * _shakePower;
            float _yAmount = Random.Range(-1f, 1f) * _shakePower;
            transform.position += new Vector3(_xAmount, _yAmount, 0f);
            _shakePower = Mathf.MoveTowards(_shakePower, 0f, _shakeFadeTime * Time.deltaTime);
            _shakeRotation = Mathf.MoveTowards(_shakeRotation, 0f, _shakeFadeTime * RotationMultiplier * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + (_shakeRotation * Random.Range(-1f, -1f)));
    }

    ///StartShake is a public function that can be called from anywhere to initiate a screen shake.
    /** The StartShake function takes two parameters, and can be called from any script in the game. When the necessary variables is changed it will trigger the LateUpdate and result in a screen shake */
    public void StartShake(float length, float power)
    {
        _shakeTimeRemaining = length;
        _shakePower = power;
        _shakeFadeTime = power / length;
        _shakeRotation = power * RotationMultiplier;
    }
}
