using System.Collections;
using UnityEngine;

/// The EnemyWeapon script is responsible for aiming and shooting the weapon of enemies.
public class EnemyWeapon : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Animator _muzzleFlashAnimation;
    [SerializeField] private string _muzzleAnimationName;
    [SerializeField] private AudioSource _shootSoundSource;
    [HideInInspector] public GameObject Target = null;

    [Header("Gun stats")]
    [SerializeField] private int _damage;
    [SerializeField] private float _bulletSpeed = 1f;
    [SerializeField] private float _minVecticalSpread;
    [SerializeField] private float _maxVerticalSpread;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _firerate;
    [SerializeField] private int _magazineAmmo;
    [SerializeField] private float _cameraShakeStrength;
    [SerializeField] private LayerMask _whatToHit;
    private int _currentAmmo;
    private float _firerateTimer = 0;
    private bool _reloading = false;

    /// Called before the first frame.
    private void Start()
    {
        _currentAmmo = _magazineAmmo;
    }

    /// Called every frame.
    /** Rotates the gun down if reloading, straight forward if no target. Aims and shoots at the target otherwise. */
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
    
    /// Aims at the specified transform.
    /** Calculates the angle between the direction of the target and the left direction. If target is below the enemy, inverts the angle. */
    public void AimAt(Transform _target)
    {
        Vector3 _direction = transform.root.position - _target.transform.position;
        float _angle = Vector3.Angle(-transform.root.right, _direction);
        if (_target.transform.position.y - transform.root.position.y < 0) _angle = -_angle;
        transform.parent.localEulerAngles = new Vector3(0, 0, _angle);
    }

    /// Shoots the gun in the aiming direction.
    /** If the firerate timer has ended, instantiate a bullet with the gun stats of the enemy. If ammo is depleted, reload. */
    public void Shoot()
    {
        if (_reloading) return;

        if (_firerateTimer < 0)
        {
            _muzzleFlashAnimation.Play(_muzzleAnimationName);

            _currentAmmo -= 1;
            float _randomSpread = Random.Range(_minVecticalSpread, _maxVerticalSpread);
            GameObject _instantiatedBullet = Instantiate(_bullet, _firePoint.position, _firePoint.rotation);
            _instantiatedBullet.transform.Rotate(0, 0, _randomSpread);
            _instantiatedBullet.GetComponent<Bullet>().CameraShakeStrength = _cameraShakeStrength;
            _instantiatedBullet.GetComponent<Bullet>().WhatToHit = _whatToHit;
            _instantiatedBullet.GetComponent<Bullet>().BulletSpeed = _bulletSpeed;
            _instantiatedBullet.GetComponent<Bullet>().IsEnemyBullet = true;
            _instantiatedBullet.GetComponent<Bullet>().TimeToLive = 2f;
            _instantiatedBullet.GetComponent<Bullet>().Damage = _damage;
            _instantiatedBullet.GetComponent<Bullet>().EnemyShooterObject = transform.root.gameObject;
            _shootSoundSource.Play();
            _firerateTimer = _firerate;
        }
        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            _reloading = true;
        }
    }

    /// Coroutine waiting for _reloadTime amount of time to reload.
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _magazineAmmo;
        _reloading = false;
    }
}
