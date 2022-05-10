using UnityEngine;

/// This script makes grappling points able to turn themselves on and off.
public class GrapplingPoint : MonoBehaviour
{
    private GrapplingHookController _grapplingHookController;
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

    /// Called before the first frame.
    /** In the Start method, we get the necessary components. 
        If the point is timed, we set it to start deactivating 1 second before it should be turned off. */
    void Start()
    {
        _grapplingHookController = GameObject.Find("Main_Character").GetComponent<GrapplingHookController>();
        _spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        if(_timed)
        {
            Invoke("StartDeactivatePoint", _timeActive - 1 + _offset);
        }
    }

    /// This method switches the sprite of the grappling point every .2 seconds for 1 second until it is turned off.
    private void StartDeactivatePoint()
    {
        for(int i = 1; i <= 4; i++)
        {
            Invoke("SwitchSprite", i * 0.2f);
        }
        Invoke("DeactivatePoint", 1f);
    }

    /// This method turns off the grappling point. If the player is currently grappling to it, the grapple is released. After _timeInactive seconds, it turns on again.
    private void DeactivatePoint()
    {
        if (_grapplingHookController.HookedPoint == gameObject)
        {
            _grapplingHookController.ReleaseGrapple();
        }
        _spriteRenderer.sprite = _inactiveSprite;
        _collider.enabled = false;
        Invoke("ActivatePoint", _timeInactive);
    }

    /// This method activates the grappling point. After _timeActive-1 seconds, it starts turning off again.
    private void ActivatePoint()
    {
        _spriteRenderer.sprite = _activeSprite;
        _collider.enabled = true;
        Invoke("StartDeactivatePoint", _timeActive - 1);
    }

    /// This method swtiches the sprite between the active sprite and the switching sprite.
    private void SwitchSprite()
    {
        if (_spriteRenderer.sprite == _activeSprite) _spriteRenderer.sprite = _switchingSprite;
        else _spriteRenderer.sprite = _activeSprite;
    }
}
