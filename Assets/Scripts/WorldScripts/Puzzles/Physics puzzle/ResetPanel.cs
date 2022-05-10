using UnityEngine;
using System.Collections;

/// This script is used on a panel that resets the objects in its _puzzleObjects array. Various options are available.
public class ResetPanel : MonoBehaviour
{
    [SerializeField] private bool _resetPosition;
    [SerializeField] private bool _resetRotation;
    [SerializeField] private bool _resetVelocity;
    [SerializeField] private bool _resetAngularVelocity;
    [SerializeField] private GameObject[] _puzzleObjects;
    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private Sprite _deactivatedSprite;
    private bool _triggered = false;
    private SpriteRenderer _spriteRenderer;

    /// Called before the first frame.
    /** Reset the sprite to the deactivated state. */
    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _deactivatedSprite;
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If the switch has recently been triggered, do nothing. 
        Otherwise, if the player is inside the trigger and presses 'E', call the ResetObjects method and deactivate for 0.3 seconds. */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_triggered) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _spriteRenderer.sprite = _activatedSprite;
                ResetObjects();
                StartCoroutine(Deactivate());
                _triggered = true;
            }
        }
    }

    /// Reset all puzzle objects connected to the panel. What to reset depends on the options chosen on the panel.
    private void ResetObjects()
    {
        foreach (GameObject _object in _puzzleObjects)
        {
            PuzzleObject _puzzleObject = _object.GetComponent<PuzzleObject>();
            Rigidbody2D _rb = _object.GetComponent<Rigidbody2D>();
            if (_resetPosition) _object.transform.position = _puzzleObject.InitialPosition;
            if (_resetRotation) _object.transform.rotation = _puzzleObject.InitialRotation;
            if (_resetVelocity) _rb.velocity = Vector2.zero;
            if (_resetAngularVelocity) _rb.angularVelocity = 0;
        }
    }

    /// Deactivates the panel for 0.3 seconds.
    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.sprite = _deactivatedSprite;
        _triggered = false;
    }
}
