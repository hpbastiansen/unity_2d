using UnityEngine;

/// Script that manages cactus destruction.
public class CactusDestroy : MonoBehaviour
{

    /// Called on a collider entering the trigger on the gameobject.
    /** If a bullet enters the trigger, destroy the cactus and increase the cactus counter for token unlocks. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            FindObjectOfType<TokenManager>().AddCactusTokenInt();
            Destroy(gameObject);
        }
    }
}
