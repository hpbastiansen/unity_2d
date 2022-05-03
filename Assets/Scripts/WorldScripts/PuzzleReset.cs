using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleReset : MonoBehaviour
{
    [SerializeField] private BlockPuzzle _puzzleController;
    [SerializeField] private Sprite _activatedSprite;
    [SerializeField] private Sprite _deactivatedSprite;
    private bool _triggered = false;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _deactivatedSprite;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_triggered || _puzzleController.PuzzleSolved) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _spriteRenderer.sprite = _activatedSprite;
                _puzzleController.ResetPuzzle();
                Invoke("Deactivate", 0.3f);
                _triggered = true;
            }
        }
    }

    private void Deactivate()
    {
        _spriteRenderer.sprite = _deactivatedSprite;
        _triggered = false;
    }
}
