using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTouchDamage : MonoBehaviour
{
    private TokenManager _tokenManager;
    public LayerMask WhatToHit;
    public float Damage;
    public float ShieldTouchDamageCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        _tokenManager = Object.FindObjectOfType<TokenManager>();
        ShieldTouchDamageCooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if ((WhatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            EnemyTemplate _enemyTemplate = other.gameObject.GetComponent<EnemyTemplate>();

            if (_enemyTemplate != null && _tokenManager.CactusTokenActive == true && ShieldTouchDamageCooldownTimer <= 0)
            {
                _enemyTemplate.TakeDamage(Damage);
                StartCoroutine(Cooldowner());
            }
        }

    }
    private IEnumerator Cooldowner()
    {
        ShieldTouchDamageCooldownTimer = 2;
        yield return new WaitForSeconds(2f);
        ShieldTouchDamageCooldownTimer = 0;
    }
}
