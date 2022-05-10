using UnityEngine;

/// Ensures the collider on the weakpoint is enabled until the weakpoint itself is destroyed to stop the player trapping themselves.
public class BlockUntilDestroyed : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private GameObject _weakpoint;
    private bool _triggered = false;

    /// Called every frame.
    void Update()
    {
        if (_triggered) return;

        if (_weakpoint == null)
        {
            _collider.enabled = false;
            _triggered = true;
        }
    }
}
