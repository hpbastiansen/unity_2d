using UnityEngine;

/// Script for deleting an object instantly.
public class DeleteThisObject : MonoBehaviour
{
    /// Called at initialization, before all objects Start() methods.
    private void Awake()
    {
        Destroy(gameObject);
    }
}
