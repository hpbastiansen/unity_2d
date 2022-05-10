using UnityEngine;

/// This script teleports the player if they fall off the edge in the tutorial.
public class TutorialCatchFall : MonoBehaviour
{
    /// Called on a collider entering the trigger on the gameobject.
    /** If the player enters the trigger, teleport them to the specified location. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent.gameObject.transform.position = new Vector2(-36.245f, 8.96125f);
        }
    }
}
