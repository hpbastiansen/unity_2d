using UnityEngine;

/// This script triggers a Wave Controller when the player enters the trigger.
public class TriggerEnemySpawner : MonoBehaviour
{
    [SerializeField] WaveController _waveController;
    private bool _hasBeenTriggered = false;

    /// Called on a collider entering the trigger on the gameobject.
    /** If the controller has not already been triggered, trigger it. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenTriggered) return;

        if(collision.gameObject.CompareTag("Player"))
        {
            _hasBeenTriggered = true;
            _waveController.Trigger();
        }
    }
}
