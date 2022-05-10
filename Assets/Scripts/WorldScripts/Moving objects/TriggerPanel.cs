using UnityEngine;

/// This script triggers a moving terrain piece, moving it from its starting position to its ending position.
public class TriggerPanel : MonoBehaviour
{
    [SerializeField] GameObject _whatToTrigger;
    private MovingTerrain _triggerScript;
    private bool _hasBeenTriggered = false;
    [SerializeField] private Sprite _spriteActivated;

    /// Called before the first frame.
    void Start()
    {
        _triggerScript = _whatToTrigger.GetComponent<MovingTerrain>();
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If the terrain hasn't been triggered, the player is standing in the trigger and presses 'E', the moving terrain is triggered. */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_hasBeenTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponentInChildren<SpriteRenderer>().sprite = _spriteActivated;
                _hasBeenTriggered = true;
                _triggerScript.Triggered = true;
            }
        }
    }
}
