using UnityEngine;

/// All objects with this script will not be destroyed when transitioning to another scene.
public class DontDestroyWhenLoad : MonoBehaviour
{
    /// Called before the first frame.
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
