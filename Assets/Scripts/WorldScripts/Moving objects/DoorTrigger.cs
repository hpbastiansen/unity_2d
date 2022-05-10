using UnityEngine;

/// This script is placed on a trigger to open a door, either by just entering the trigger or requiring a key press inside.
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject _doorToTrigger;
    private Door _triggerScript;
    private bool _hasBeenTriggered = false;
    [SerializeField] private bool _needKeyPress = false;
    [SerializeField] private Sprite _spriteActivated;

    /// Called before the first frame.
    void Start()
    {
        _triggerScript = _doorToTrigger.GetComponent<Door>();
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If the script hasn't been triggered, check if the player is inside the trigger. 
        If they are, if it requires a key press check for it and trigger. Otherwise, just trigger. */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_hasBeenTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (_needKeyPress)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponentInChildren<SpriteRenderer>().sprite = _spriteActivated;
                    _hasBeenTriggered = true;
                    _triggerScript.Triggered = true;
                }
            }
            else
            {
                _hasBeenTriggered = true;
                _triggerScript.Triggered = true;
            }
        }
    }
}
