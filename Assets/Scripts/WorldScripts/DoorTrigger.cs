using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject _doorToTrigger;
    private Door _triggerScript;
    private bool _hasBeenTriggered = false;
    [SerializeField] private bool _needKeyPress = false;
    [SerializeField] private Sprite _spriteActivated;

    // Start is called before the first frame update
    void Start()
    {
        _triggerScript = _doorToTrigger.GetComponent<Door>();
    }

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
