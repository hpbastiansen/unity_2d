using UnityEngine;

/// This script adds a stage checkpoint to the player on entering the trigger.
public class AddStageCheckPoint : MonoBehaviour
{
    public bool CanAddCheckPoint;

    /// Called before the first frame.
    void Start()
    {
        CanAddCheckPoint = true;
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** When the player enters this trigger, add the checkpoint to the checkpoint manager, then disable itself */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StageCheckPointManager _checkpointManager = FindObjectOfType<StageCheckPointManager>();
            _checkpointManager.AddCheckpoint(gameObject);
            gameObject.SetActive(false);
        }
    }
}
