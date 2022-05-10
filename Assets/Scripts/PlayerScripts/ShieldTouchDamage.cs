using System.Collections;
using UnityEngine;

/// This script enables the shield to deal touch damage if the player has the cactus token active.
public class ShieldTouchDamage : MonoBehaviour
{
    private TokenManager _tokenManager;
    public LayerMask WhatToHit;
    public float Damage;
    public float ShieldTouchDamageCooldownTimer;
    private bool _onCooldown;

    /// Called before the first frame.
    void Start()
    {
        _tokenManager = FindObjectOfType<TokenManager>();
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** If the collider is on one of the layers specified, try to get their EnemyHealth component. 
        If it exists and the player is using the cactus token and the cooldown is over, deal damage to the enemy and start the cooldown timer. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onCooldown) return;
        if ((WhatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            EnemyHealth _enemyHealth = other.gameObject.GetComponent<EnemyHealth>();

            if (_enemyHealth != null && _tokenManager.CactusTokenActive == true)
            {
                _enemyHealth.TakeDamage(Damage);
                StartCoroutine(Cooldowner());
            }
        }
    }

    /// Wait for the cooldown, then set _onCooldown to false.
    private IEnumerator Cooldowner()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(ShieldTouchDamageCooldownTimer);
        _onCooldown = false;
    }
}
