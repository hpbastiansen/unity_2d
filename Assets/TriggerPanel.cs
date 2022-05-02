using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPanel : MonoBehaviour
{
    [SerializeField] GameObject _whatToTrigger;
    private MovingTerrain _triggerScript;
    private bool _hasBeenTriggered = false;
    [SerializeField] private Sprite _spriteActivated;

    // Start is called before the first frame update
    void Start()
    {
        _triggerScript = _whatToTrigger.GetComponent<MovingTerrain>();
    }

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