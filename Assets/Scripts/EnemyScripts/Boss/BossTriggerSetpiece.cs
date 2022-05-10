using UnityEngine;

/// This script is used for updating the boss' "Sections done" counter when the player enters the trigger on the gameobject this is attached to.
public class BossTriggerSetpiece : MonoBehaviour
{
    private bool _hasBeenTriggered = false;
    private BossController _controller;

    /// Called before the first frame.
    private void Start()
    {
        _controller = GameObject.Find("BossController").GetComponent<BossController>();
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** If the player is the collider entering, increase the counter and tell the boss to spawn the next section. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            _controller.SectionsDone++;
            _controller.ReadyForNextSection = true;
            _hasBeenTriggered = true;
        }
    }
}
