using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpoint : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private Sprite _healthySprite;
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private Sprite _veryDamagedSprite;
    private BossController _bossController;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bossController = GameObject.Find("BossController").GetComponent<BossController>();
        float _currentHealthRatio = _bossController.Health / _bossController.MaxHealth;
        if (_currentHealthRatio > 0.66) _spriteRenderer.sprite = _healthySprite;
        else if (_currentHealthRatio > 0.33) _spriteRenderer.sprite = _damagedSprite;
        else _spriteRenderer.sprite = _veryDamagedSprite;
    }

    void Update()
    {
        if (_health <= 0)
        {
            GameObject.Find("BossController").GetComponent<BossController>().WeakpointDestroyed();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        _health = _health - dmg;
    }
}
