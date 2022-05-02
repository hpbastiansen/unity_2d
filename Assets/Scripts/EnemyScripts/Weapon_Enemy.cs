using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

///This is basically the same script as the Player weapon script. All necessary documentation should be in there.
public class Weapon_Enemy : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject Bullet;
    public GameObject Target = null;
    
    [SerializeField] private int _damage;
    [SerializeField] private float _bulletSpeed = 1f;
    [SerializeField] private float _minVecticalSpread;
    [SerializeField] private float _maxVerticalSpread;

    private bool _reloading = false;

    [SerializeField] private int _magazineAmmo;
    private int _currentAmmo;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _firerate;
    private float _firerateTimer = 0;
    
    public float CameraShakeStrength = 1;
    public Animator MuzzleFlashAnimation;
    public string MuzzleAnimationName;
    public LayerMask WhatToHit;

    public AudioSource ShootSoundSource;

    private void Start()
    {
        _currentAmmo = _magazineAmmo;
    }

    private void Update()
    {
        if(_reloading)
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, -45f);
        }
        else if(Target != null)
        {
            AimAt(Target.transform);
            Shoot();
        }
        else
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (_firerateTimer >= 0) _firerateTimer -= Time.deltaTime;
    }
    
    public void AimAt(Transform _target)
    {
        Vector3 _direction = transform.root.position - _target.transform.position;
        float _angle = Vector3.Angle(-transform.root.right, _direction);
        if (_target.transform.position.y - transform.root.position.y < 0) _angle = -_angle;
        transform.parent.localEulerAngles = new Vector3(0, 0, _angle);
    }

    public void Shoot()
    {
        if (_reloading) return;

        if (_firerateTimer < 0)
        {
            MuzzleFlashAnimation.Play(MuzzleAnimationName);

            _currentAmmo -= 1;
            float _randomSpread = Random.Range(_minVecticalSpread, _maxVerticalSpread);
            GameObject _theBullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            _theBullet.transform.Rotate(0, 0, _randomSpread);
            _theBullet.GetComponent<Bullet>().CameraShakeStrength = CameraShakeStrength;
            _theBullet.GetComponent<Bullet>().WhatToHit = WhatToHit;
            _theBullet.GetComponent<Bullet>().BulletSpeed = _bulletSpeed;
            _theBullet.GetComponent<Bullet>().IsEnemyBullet = true;
            _theBullet.GetComponent<Bullet>().TimeToLive = 2f;
            _theBullet.GetComponent<Bullet>().Damage = _damage;
            _theBullet.GetComponent<Bullet>().EnemyShooterObject = transform.root.gameObject;
            ShootSoundSource.Play();
            _firerateTimer = _firerate;
        }
        if (_currentAmmo <= 0)
        {
            Invoke("Reload", _reloadTime);
            _reloading = true;
        }
    }

    private void Reload()
    {
        _currentAmmo = _magazineAmmo;
        _reloading = false;
    }
}
