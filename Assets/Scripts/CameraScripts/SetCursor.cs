//////////////////////////////////////////////////////////////////////////////////////////////////
/* https://wintermutedigital.com/post/2020-01-29-the-ultimate-guide-to-custom-cursors-in-unity/ */
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///The SetCursor script sets a custom ingame cursor for the game.
public class SetCursor : MonoBehaviour
{
    // You must set the cursor in the inspector.
    public Texture2D Crosshair;


    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we first set the cursor origin to its centre. (default is upper left corner). Then we set the cursor to the Crosshair sprite with given offset.
    And lastly automatic switching to hardware default if necessary*/
    void Start()
    {
        Vector2 _cursorOffset = new Vector2(Crosshair.width / 2, Crosshair.height / 2);
        Cursor.SetCursor(Crosshair, _cursorOffset, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }

}