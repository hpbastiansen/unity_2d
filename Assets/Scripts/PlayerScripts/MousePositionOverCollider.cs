using UnityEngine;

/// This script creates a method that checks if the mouse is on a 'Ground' object or not.
public class MousePositionOverCollider : MonoBehaviour
{
    Camera cam;

    /// Cast a ray from the camera to the mouse position. If the ray hits an object tagged with 'Ground', return true. Otherwise return false.
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
