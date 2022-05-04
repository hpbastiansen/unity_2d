//https://github.com/Brackeys/Smooth-Camera-Follow/blob/master/Smooth%20Camera/Assets/CameraFollow.cs
using UnityEngine;

///The CamerFollower script makes the camer follow another objects position with a smooth effect.
public class CameraFollower : MonoBehaviour
{

    public Transform ObjectToFollow;

    public float CameraSmoothSpeed = 0.02f;

    public Vector3 Offset;
    public Movement PlayerMovement;



    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the start function the player movement script is found and assigned to a variable, and a desired offset value for the camera is set. */
    void Start()
    {
        PlayerMovement = GameObject.Find("Main_Character").GetComponent<Movement>();
        Offset = new Vector3(0, .83f, -25f);
    }

    ///Fixed Update is called based on a fixed frame rate.
    /**FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame. Compute Physics system calculations after FixedUpdate. 0.02 seconds (50 calls per second) is the default time between calls.*/
    /*!In the Fixed Update function the desired position of the camera is calculated based on which object to follow and the previously set offset value. Then a "smoothed" position is set 
    by using the Lerp function which linearly interpolates between two points. Mathematically it equals to "a + (b - a) * t". Where the a is the current position of the camera object, b is the desired position (with offset), 
    and t is the how fast we want the camera smoohting to be (variable "CameraSmoothSpeed")*/
    void FixedUpdate()
    {
        Vector3 _desiredPosition = ObjectToFollow.position + Offset;
        Vector3 _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, CameraSmoothSpeed);
        transform.position = _smoothedPosition;
    }

}