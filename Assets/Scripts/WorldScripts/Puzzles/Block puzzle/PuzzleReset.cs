using UnityEngine;
using System.Collections;

/// This script enables the player to reset the block puzzle on stage 4.
public class PuzzleReset : MonoBehaviour
{
    [SerializeField] private BlockPuzzle _puzzleController;
    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private Sprite _deactivatedSprite;
    private bool _triggered = false;
    private SpriteRenderer _spriteRenderer;

    /// Called before the first frame.
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _deactivatedSprite;
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If the puzzle is solved or switch is already triggered, do nothing.
        Else, if the player is pressing 'E', reset the puzzle to its original state and disable the switch for 0.3 seconds. */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_triggered || _puzzleController.PuzzleSolved) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _puzzleController.ResetPuzzle();
                StartCoroutine(ActivateSwitchForSeconds(0.3f));
            }
        }
    }

    /// Keeps the switch activated for a specified amount of seconds.
    IEnumerator ActivateSwitchForSeconds(float _time)
    {
        _spriteRenderer.sprite = _activatedSprite;
        _triggered = true;
        yield return new WaitForSeconds(_time);
        _spriteRenderer.sprite = _deactivatedSprite;
        _triggered = false;
    }
}
