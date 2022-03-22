using UnityEngine;

///This script makes a game object unable to rotate.
public class NoRotation : MonoBehaviour
{
    ///Update is called every frame
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often*/
    /*! In the Update function we contantly set the rotation to 0 degrees on all axes.*/
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }
}
