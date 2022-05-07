using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

///The Bullet script is a universal script used for a projectile, with means of hitting an enemy or player.
public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 20f;
    public Rigidbody2D BulletRigidbody;
    public float Damage = 10;
    public GameObject HitEffect;
    public GameObject HitShieldEffect;

    public GameObject BlockEffect;
    public LayerMask WhatToHit;
    public float CameraShakeStrength = 0;
    public float TimeToLive = 1f;
    public bool IsEnemyBullet;
    public bool UsingCactusToken = false;
    public GameObject EnemyShooterObject;
    private bool _triggered;

    [Header("Homing")]
    public bool IsHoming;
    public GameObject CactusSplinters;

    [Header("Lifesteal settings")]
    public float LifeSteal = 0;
    public PlayerHealth PlayerHealthScript;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /**The Start function assigns the velocity and the direction of the bullet. Then starts the coroutine to destroy the bullet, which works as a independent timer.
    And lastly finds and assigs the PlayerHealth script.*/
    void Start()
    {
        if (IsHoming == false)
        {
            BulletRigidbody.velocity = transform.right * BulletSpeed;
        }
        StartCoroutine(DestoryBullet());
        PlayerHealthScript = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();
        UsingCactusToken = Object.FindObjectOfType<TokenManager>().CactusTokenActive;
    }
    private void Update()
    {
        if (IsHoming == true)
        {
            EnemyDistance closestEnemy = EnemyDistance.FindNearest(transform.position);
            if (closestEnemy != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, Time.deltaTime * BulletSpeed);
            }
            else
            {
                BulletRigidbody.velocity = transform.right * BulletSpeed;
            }
        }
    }
    ///Function will run when an incoming collider makes contact with this object's collider.
    /**In this function we check what the bullet hits. If the bullet hits another collider with a specific layer, we either;
    Call the TakeDamage function on the player, enemy or shield. 
    Either way the script instantiate (spawns/creates) a new hiteffect (explotion animation) as a gameobject based on a prefab. 
    Then the StartShake function on the ScreenShakeController script is called to make the screen shake depending on what it hit (only if the player is in x distance, see _distance variable). 
    Lastly we destroy the bullet, aka the gameObject that has this script attached to it.*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        float _distance = Vector3.Distance(PlayerHealthScript.transform.position, transform.position);
        if ((WhatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer && other.isTrigger == false) //http://answers.unity.com/answers/1394106/view.html
        {
            EnemyHealth _enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            EnemyAI _enemyAI = other.gameObject.GetComponent<EnemyAI>();
            PlayerHealth _playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            ShieldHP _shieldHP = other.gameObject.GetComponent<ShieldHP>();
            WormWeakpoint _wormWeakpoint = other.gameObject.GetComponent<WormWeakpoint>();
            CrackedBlock _crackedBlock = other.gameObject.GetComponent<CrackedBlock>();
            BossWeakpoint _bossWeakpoint = other.gameObject.GetComponent<BossWeakpoint>();

            if (_enemyHealth != null)
            {
                if(_enemyAI != null) other.gameObject.GetComponent<EnemyAI>().CurrentPhase = EnemyAI.AIPhase.Pursuit;
                _enemyHealth.TakeDamage(Damage);
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                ScreenShakeController.Instance.StartShake(.05f, .03f, true);
                PlayerHealthScript.CurrentHP += LifeSteal;
                Destroy(gameObject);
            }
            else if (_playerHealth != null)
            {
                float _angle = _playerHealth.transform.position.x - transform.position.x > 0 ? 45f : 135f;

                _playerHealth.TakeDamage(Damage, 10f, _angle);
                if (_playerHealth.IsBlocking == true)
                {
                    if (_triggered) return;
                    _triggered = true;
                    Instantiate(BlockEffect, transform.position, BlockEffect.transform.rotation);
                    Destroy(gameObject);
                    if(SceneManager.GetActiveScene().name == "TUTORIAL") GameObject.Find("TutorialManager").GetComponent<TutorialManager>().ShotCountered();
                }
                else
                {
                    Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                    if (_distance < 10)
                    {
                        ScreenShakeController.Instance.StartShake(.2f, .1f, true);
                    }
                    Destroy(gameObject);
                }
            }
            else if (_shieldHP != null)
            {
                if (SceneManager.GetActiveScene().name == "TUTORIAL") GameObject.Find("TutorialManager").GetComponent<TutorialManager>().ShotBlocked();
                _shieldHP.TakeDamage(Damage);
                Instantiate(HitShieldEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                TokenManager _tokenManager = Object.FindObjectOfType<TokenManager>();
                if (_tokenManager.WormTokenActive && IsEnemyBullet)
                {
                    EnemyShooterObject.GetComponent<EnemyHealth>().Debuff();
                }
                if (_distance < 10)
                {
                    ScreenShakeController.Instance.StartShake(.2f, .1f, true);
                }
                Destroy(gameObject);
            }
            else if (_wormWeakpoint != null)
            {
                _wormWeakpoint.TakeDamage(Damage);
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                ScreenShakeController.Instance.StartShake(.05f, .03f, true);
                PlayerHealthScript.CurrentHP += LifeSteal;
                Destroy(gameObject);
            }
            else if(_crackedBlock != null)
            {
                _crackedBlock.OnHit();
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                ScreenShakeController.Instance.StartShake(.05f, .03f, true);
                Destroy(gameObject);
            }
            else if(_bossWeakpoint != null)
            {
                _bossWeakpoint.TakeDamage(Damage);
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                ScreenShakeController.Instance.StartShake(.05f, .03f, true);
                PlayerHealthScript.CurrentHP += LifeSteal;
                Destroy(gameObject);
            }
            else
            {
                if (_distance < 10)
                {
                    ScreenShakeController.Instance.StartShake(.05f, .03f, true);
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
        for (; ; )
        {
            if (UsingCactusToken == true && IsEnemyBullet == false)
            {
                BulletRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY; BulletRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX; BulletRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                GameObject thesplinters = Instantiate(CactusSplinters, gameObject.transform.position, gameObject.transform.rotation);
                Component[] thesplintersbullet;
                thesplintersbullet = GetComponentsInChildren<Bullet>();
                foreach (Bullet splinter in thesplintersbullet)
                {
                    UsingCactusToken = false;
                }

                Destroy(gameObject);
            }
            else if (UsingCactusToken == false)
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}