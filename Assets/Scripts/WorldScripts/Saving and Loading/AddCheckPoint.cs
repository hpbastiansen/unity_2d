using UnityEngine;

/// This script adds a checkpoint to the player if they do not already have it activated.
public class AddCheckPoint : MonoBehaviour
{
    private CheckPointManager _checkpointManager;
    public string CheckPointToAdd;
    public bool W1;
    public bool W2;
    public bool W3;

    /// Called before the first frame.
    private void Start()
    {
        _checkpointManager = FindObjectOfType<CheckPointManager>();
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** When the player enters the trigger, the checkpoint is added to the world specified on the script. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (W1)
            {
                if (!_checkpointManager.W1Scenes.Contains(CheckPointToAdd))
                {
                    _checkpointManager.AddCheckPointW1(CheckPointToAdd);
                }
            }
            if (W2)
            {
                if (!_checkpointManager.W2Scenes.Contains(CheckPointToAdd))
                {
                    _checkpointManager.AddCheckPointW2(CheckPointToAdd);
                }
            }
            if (W3)
            {
                if (!_checkpointManager.W3Scenes.Contains(CheckPointToAdd))
                {
                    _checkpointManager.AddCheckPointW3(CheckPointToAdd);
                }
            }

        }
    }
}
