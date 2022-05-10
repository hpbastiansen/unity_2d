using System.Collections;
using UnityEngine;

/// This script is used to make lights sources flicker after a random amount of time.
public class LightTubeFlicker : MonoBehaviour
{
    private Animator _myAnimator;
    private bool _isFlickering;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! The Start function finds the Animator component connected to this gameObject. */
    void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }

    /// Update is called every frame.
    /** The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often. */
    /*! The Update function is first checking if the light is already flickering, if its not it will generate a random number for it to wait to flicker,
    and then call the Flicker coroutine. */
    void Update()
    {
        if (_isFlickering == false)
        {
            _isFlickering = true;
            float _randomWaitTime;
            _randomWaitTime = Random.Range(1, 10);
            StartCoroutine(Flicker(_randomWaitTime));
        }
    }

    /// An IEnumerator to randomize the flicker wait time.
    /** Random wait time for the light flicker, so that all lights does not flicker at the same time. */
    IEnumerator Flicker(float _randomWaitTime)
    {
        yield return new WaitForSeconds(_randomWaitTime);
        _myAnimator.SetBool("Flicker", true);
        yield return new WaitForEndOfFrame();
        _myAnimator.SetBool("Flicker", false);
        _isFlickering = false;
    }
}
