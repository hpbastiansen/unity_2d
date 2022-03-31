using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionOverCollider : MonoBehaviour
{
    Camera cam;
    public bool CheckForCollider()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider == null)
        {
            return (false);
        }
        else
        {
            if (hit.collider.tag == "Ground")
            {
                return (true);
            }
            else
            {
                return (false);
            }

        }
    }
}
