using UnityEngine;

/// The CheckForGround script checks if the player is touching the ground or not. If CheckForRoof is true, the same applies for the roof instead.
public class CheckForGround : MonoBehaviour
{
    private Movement _playerMovement;
    public LayerMask WhatToHit;
    public bool CheckForRoof;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! In the Start function the player Movement script is located. */
    void Start()
    {
        _playerMovement = transform.parent.GetComponent<Movement>();
    }

    /// Sent each frame where another object is within a trigger collider attached to this object 
    /** Whenever the object carrying this script has its 2D collider (with isTrigger enabled) inside another collider with a given LayerMask we change the variable "IsTouchingGround" or "IsCloseToRoof" in player Movement script to true.
    This allows the player to jump if jump key is pressed. */
    void OnTriggerStay2D(Collider2D other)
    {
        if ((WhatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (CheckForRoof)
            {
                _playerMovement.IsCloseToRoof = true;
            }
            else
            {
                _playerMovement.IsTouchingGround = true;                
            }
        }
    }

    /// Sent when another object leaves a trigger collider attached to this object.
    /** Whenever the 2D collider (with isTrigger enabled) of object carrying this script leaves another collider with a given LayerMask we change the variable "IsTouchingGround" or "IsCloseToRoof" in player Movement script to false.
    Not allowing the player to jump. This is to only allow player to jump whenever he is touching the ground and not too close to the roof. */
    void OnTriggerExit2D(Collider2D other)
    {
        if ((WhatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (CheckForRoof)
            {
                _playerMovement.IsCloseToRoof = false;
            }
            else
            {
                _playerMovement.IsTouchingGround = false;
            }
        }
    }
}