using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingPoint : MonoBehaviour
{
    [SerializeField] private bool _timed = false;
    [SerializeField] private float _timeActive;
    [SerializeField] private float _timeInactive;
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _switchingSprite;
    [SerializeField] private Sprite _inactiveSprite;
    [SerializeField] private float _offset;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;
    public float MaxDistance;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        if(_timed)
        {
            Invoke("StartDeactivatePoint", _timeActive - 1 + _offset);
        }
    }

    private void StartDeactivatePoint()
    {
        for(int i = 1; i <= 4; i++)
        {
            Invoke("SwitchSprite", i * 0.2f);
        }
        Invoke("DeactivatePoint", 1f);
    }

    private void DeactivatePoint()
    {
        // Stop grappling hook if hooked to this point.
        _spriteRenderer.sprite = _inactiveSprite;
        _collider.enabled = false;
        Invoke("ActivatePoint", _timeInactive);
    }

    private void ActivatePoint()
    {
        _spriteRenderer.sprite = _activeSprite;
        _collider.enabled = true;
        Invoke("StartDeactivatePoint", _timeActive - 1);
    }

    private void SwitchSprite()
    {
        if (_spriteRenderer.sprite == _activeSprite) _spriteRenderer.sprite = _switchingSprite;
        else _spriteRenderer.sprite = _activeSprite;
    }
}
