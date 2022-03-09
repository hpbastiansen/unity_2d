using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
///The TokenChange script is a script assigned to the UI indication of a token change. Only purpose is to delete it after 2 seconds.
public class TokenChange : MonoBehaviour
{
    public Text txt;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    void Start()
    {
        txt = GetComponentInChildren<Text>();
    }
    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! Only purpose is to delete it after two seconds, this is to prevent a stack of unused and invisible notifications to slow down the game.*/
    void Update()
    {
        Destroy(gameObject, 2);
    }
}
