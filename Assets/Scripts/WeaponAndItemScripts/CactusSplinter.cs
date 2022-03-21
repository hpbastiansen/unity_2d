using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

///The CactusSplinter script is a script based upon the Bullet script, but this is used for projectiles shot from the bullet when using the cactus token.
public class CactusSplinter : MonoBehaviour
{
    public float BulletSpeed = 20f;
    public Rigidbody2D BulletRigidbody;
    public float Damage = 10;
    public GameObject HitEffect;
    public GameObject BlockEffect;
    public LayerMask WhatToHit;
    public float CameraShakeStrength = 0;
    public float TimeToLive = 1f;
    public bool IsEnemyBullet;
    public PlayerHealth PlayerHealthScript;
    private TokenManager _tokenManager;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /**The Start function assigns the velocity and the direction of the bullet. Then starts the coroutine to destroy the bullet, which works as a independent timer.
    And lastly finds and assigs the Damage variable and TokenManger and PlayerHealth script.*/
    void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody2D>();
        BulletRigidbody.velocity = transform.right * BulletSpeed;
        _tokenManager = Object.FindObjectOfType<TokenManager>();
        StartCoroutine(DestoryBullet());
        PlayerHealthScript = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();
        Damage = (_tokenManager.CurrentWeapon.Damage);
    }

    ///Function will run when an incoming collider makes contact with this object's collider.
    /**In this function we check what the bullet hits. If the bullet hits another collider with a specific layer, we
    call the TakeDamage function on the enemy. 
    The script instantiate (spawns/creates) a new hiteffect (explotion animation) as a gameobject based on a prefab. 
    Then the StartShake function on the ScreenShakeController script is called to make the screen shake depending on what it hit (only if the player is in x distance, see _distance variable). 
    Lastly we destroy the bullet, aka the gameObject that has this script attached to it.*/
    private void OnCollisionEnter2D(Collision2D other)
    {
        float _distance = Vector3.Distance(PlayerHealthScript.transform.position, transform.position);

        if ((WhatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) //http://answers.unity.com/answers/1394106/view.html
        {
            EnemyTemplate _enemyTemplate = other.gameObject.GetComponent<EnemyTemplate>();
            PlayerHealth _playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            ShieldHP _shieldHP = other.gameObject.GetComponent<ShieldHP>();

            if (_enemyTemplate != null)
            {
                _enemyTemplate.TakeDamage(Damage);
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));

                ScreenShakeController.Instance.StartShake(.05f, .03f);

                Destroy(gameObject);
            }
            else
            {
                if (_distance < 10)
                {
                    ScreenShakeController.Instance.StartShake(.05f, .03f);
                }
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                Destroy(gameObject);
            }

        }
    }

    ///An independent IEnumerator that is called in the Start function to destorys the gameobject after x seconds.
    IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(TimeToLive);
        Destroy(gameObject);
    }
}
