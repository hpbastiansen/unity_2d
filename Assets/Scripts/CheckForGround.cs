using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGround : MonoBehaviour
{
    private Movement parent;
    public LayerMask whattohit;

    void Start()
    {
        parent = transform.parent.GetComponent<Movement>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((whattohit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            parent.isTouchingGround = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if ((whattohit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            parent.isTouchingGround = false;
        }
    }
}