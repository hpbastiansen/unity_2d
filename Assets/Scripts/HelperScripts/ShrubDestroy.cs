using UnityEngine;

/// Script that manages shrub destruction.
public class ShrubDestroy : MonoBehaviour
{

    /// Called on a collider entering the trigger on the gameobject.
    /** If a bullet enters the trigger, destroy the shrub and increase the shrub counter for token unlocks. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            FindObjectOfType<TokenManager>().AddShrubsTokenInt();
            Destroy(gameObject);
        }
    }
}