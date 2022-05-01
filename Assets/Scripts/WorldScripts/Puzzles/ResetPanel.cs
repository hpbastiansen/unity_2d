using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = _deactivatedSprite;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_triggered) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _spriteRenderer.sprite = _activatedSprite;
                ResetObjects();
                Invoke("Deactivate", 0.3f);
                _triggered = true;
            }
        }
    }

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

    private void Deactivate()
    {
        _spriteRenderer.sprite = _deactivatedSprite;
        _triggered = false;
    }
}
