using UnityEngine;

/// This script is used for drawing a visual representation of a pathfinding node in the editor.
public class NodeGizmo : MonoBehaviour
{
    /// Draw a green wire sphere on the position of the gameobject.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
