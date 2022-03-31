using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

///This is basically the same script as the Player weapon script. All necessary documentation should be in there.
public class Weapon_Enemy : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject Bullet;
    public float Speedofbullet = 1f;
    public float MinVecticalSpread = 0f;
    public float MaxVerticalSpread = 0f;

    public int Ammo = 1;
    public bool MaxAmmo;
    public float Firerate;
    public float FirerateCouter = 0;

    public float CameraShakeStrength = 1;
    public Animator MuzzleFlashAnimation;
    public string MuzzleAnimationName;
    public LayerMask WhatToHit;

    ///This is basically the same script as the Player weapon script. All necessary documentation should be in there.
    private void Start()
    {
        ShootFullAuto();
    }

    void LateUpdate()
    {
        if (MaxAmmo == true)
        {
            Ammo = 100000;
        }

        if (FirerateCouter >= 0)
        {
            FirerateCouter -= Time.deltaTime;
        }
    }
    void Shoot()
    {

        if (Ammo > 0)
        {
            MuzzleFlashAnimation.Play(MuzzleAnimationName);

            Ammo -= 1;
            float _randomSpread = Random.Range(MinVecticalSpread, MaxVerticalSpread);
            GameObject _theBullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            _theBullet.transform.Rotate(0, 0, _randomSpread);
            _theBullet.GetComponent<Bullet>().CameraShakeStrength = CameraShakeStrength;
            _theBullet.GetComponent<Bullet>().WhatToHit = WhatToHit;
            _theBullet.GetComponent<Bullet>().BulletSpeed = Speedofbullet;
            _theBullet.GetComponent<Bullet>().IsEnemyBullet = true;
            _theBullet.GetComponent<Bullet>().TimeToLive = 2f;
            _theBullet.GetComponent<Bullet>().EnemyShooterObject = transform.root.gameObject;


        }
    }
    void ShootFullAuto()
    {


        if (Ammo > 0)
        {
            MuzzleFlashAnimation.Play(MuzzleAnimationName);

            Ammo -= 1;
            float _randomSpread = Random.Range(MinVecticalSpread, MaxVerticalSpread);
            GameObject _theBullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
            _theBullet.transform.Rotate(0, 0, _randomSpread);
            _theBullet.GetComponent<Bullet>().CameraShakeStrength = CameraShakeStrength;
            _theBullet.GetComponent<Bullet>().WhatToHit = WhatToHit;
            _theBullet.GetComponent<Bullet>().BulletSpeed = Speedofbullet;
            _theBullet.GetComponent<Bullet>().IsEnemyBullet = true;
            _theBullet.GetComponent<Bullet>().TimeToLive = 2f;
            _theBullet.GetComponent<Bullet>().EnemyShooterObject = transform.root.gameObject;


        }
        StartCoroutine(FullAutoCooldown());

    }
    IEnumerator FullAutoCooldown()
    {
        FirerateCouter = Firerate;
        yield return new WaitForSeconds(Firerate);
        ShootFullAuto();
    }

}
