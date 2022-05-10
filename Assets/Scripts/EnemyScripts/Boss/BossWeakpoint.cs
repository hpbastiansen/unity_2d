using UnityEngine;

/// This script allows the boss' weakpoint to be damaged.
public class BossWeakpoint : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private Sprite _healthySprite;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private Sprite _veryDamagedSprite;
    private BossController _bossController;
    private SpriteRenderer _spriteRenderer;

    /// Called before the first frame update.
    /** In the start function we set the sprite based on the boss' current health. The sprite has 3 different versions. */
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bossController = GameObject.Find("BossController").GetComponent<BossController>();
        float _currentHealthRatio = _bossController.Health / _bossController.MaxHealth;
        if (_currentHealthRatio > 0.66) _spriteRenderer.sprite = _healthySprite;
        else if (_currentHealthRatio > 0.33) _spriteRenderer.sprite = _damagedSprite;
        else _spriteRenderer.sprite = _veryDamagedSprite;
    }

    /// Called every frame.
    /** Destroy the gameobject if health is below 0, and start the "Weakpoint Destroyed" sequence of the boss. */
    void Update()
    {
        if (_health <= 0)
        {
            GameObject.Find("BossController").GetComponent<BossController>().WeakpointDestroyed();
            Destroy(gameObject);
        }
    }

    /// Other scripts can call this function to lower health.
    public void TakeDamage(float dmg)
    {
        _health = _health - dmg;
    }
}
