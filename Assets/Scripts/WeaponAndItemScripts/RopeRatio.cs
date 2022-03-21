//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*https://www.youtube.com/watch?v=sHhzWlrTgJo&list=PUuXkaW-PS6zmJ5zO4FbiiXQ&index=2&ab_channel=DonHaulGameDev-Wabble-UnityTutorials*/
/*https://www.youtube.com/watch?v=DTFgQIs5iMY&list=PUuXkaW-PS6zmJ5zO4FbiiXQ&index=11&ab_channel=DonHaulGameDev-Wabble-UnityTutorials*/
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

///The RopeRatio script's purpose is to correctly change the grapplinghook line's scale on the X axis.
public class RopeRatio : MonoBehaviour
{
    public GameObject PlayerObject;
    public Vector3 PositionToHook;
    public float Ratio;

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! The Update function defines a float for the scale on the X axis that is based upon the Vector2 distance from the player object to the hook position, 
    this is divided by the Ratio variable, which is how thick we want the rope to be.
    Lastly we find the LineRenderer component and apply the values.*/
    void Update()
    {
        float _scaleX = Vector3.Distance(PlayerObject.transform.position, PositionToHook) / Ratio;
        GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(_scaleX, 1f);
    }
}
