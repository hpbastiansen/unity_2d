using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

///The purpose of the Weapon script is to make a versatile template for most "normal" guns.
public class Weapon : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject BulletObject;
    public float Damage;
    public float BulletSpeed = 1f;
    public float MinVerticalSpread = 0f;
    public float MaxVerticalSpread = 0f;
    public float BulletWeight = 1;
    public string WeaponType = "Rifle";
    public int Ammo = 1;
    public bool FullAuto;
    public bool IsShootingFullAuto;
    public float Firerate;
    public float FirerateCounter = 0;
    public float BulletTimeToLive = 1;
    public float CameraShakeStrength = 1;
    public Animator MuzzleFlashAnimator;
    public string MuzzleFlashAnimationName;
    public LayerMask WhatToHit;
    private TokenManager TokenManagerScript;

    public float LifeSteal = 0;

    public Sprite WeaponSymbol;
    private Image ImageUI;
    private Text AmmoText;
    public DialogueManager DialogueManagerScript;
    private UITest _checkUI;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function several variables are set, and script, components and objects are found and assigned.*/
    void Start()
    {
        TokenManagerScript = GameObject.Find("GameManager").GetComponent<TokenManager>();
        LifeSteal = TokenManagerScript.GunLifeStealAmount;
        AmmoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();
        DialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
        _checkUI = Object.FindObjectOfType<UITest>();

    }

    /// LateUpdate is called every frame
    /** LateUpdate is called after all Update functions have been called. This is useful to order script execution.*/
    /*! The Late Update allows for shooting in both FullAuto and SemiAutomatic. 
    The Late Update also makes sure to update the Ammo UI text at the end.*/
    async void LateUpdate()
    {
        if (FullAuto == true)
        {
            if (Input.GetMouseButton(0) && Time.time >= FirerateCounter && _checkUI.IsPointerOverUIElement() == false)
            {
                FirerateCounter = Time.time + 1f / Firerate;
                Shoot();
            }
        }
        else if (FullAuto == false)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= FirerateCounter && _checkUI.IsPointerOverUIElement() == false)
            {
                FirerateCounter = Time.time + 1f / Firerate;
                Shoot();
            }
        }
        AmmoText.text = Ammo.ToString();
    }

    ///This function is called when the object becomes enabled and active.
    /**Whenever the weapon is enables it locates and changes the visual UI Image representation on the weapon and the current Ammo situation.
    It also makes sure the gun is not already shooting.*/
    private void OnEnable()
    {
        ImageUI = GameObject.Find("currentWepImg").GetComponent<Image>();
        IsShootingFullAuto = false;
        ImageUI.sprite = WeaponSymbol;
        AmmoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();
        AmmoText.text = Ammo.ToString();
    }


    ///The Shoot function allows the gun to shoot SemiAuto.
    /**The function checks if the Ammo amount is above 0.
    If true, it gathers information about the current tokens lifesteal stats, subtracts one from the ammo value, generates a random vertical spread,
    plays a muzzleflash animation (light at the end of the gun when shooting), instantiates a bullet gameobject based on a prefab, and set its correct location to be at the end of the gunbarrel.
    then it sets the rotation of the bullet to be equal the _randomSpread and lastly assigns necessary values to the bullet 
    (e.g. lifesteal, camerashake strength, what it can hit, bulletspeed and damage).*/
    void Shoot()
    {
        if (Ammo > 0)
        {
            LifeSteal = TokenManagerScript.GunLifeStealAmount;
            MuzzleFlashAnimator.Play(MuzzleFlashAnimationName);
            Ammo -= 1;
            float _randomSpread = Random.Range(MinVerticalSpread, MaxVerticalSpread);
            GameObject thebullet = Instantiate(BulletObject, FirePoint.position, FirePoint.rotation);
            thebullet.transform.Rotate(0, 0, _randomSpread);
            thebullet.GetComponent<Bullet>().CameraShakeStrength = CameraShakeStrength;
            thebullet.GetComponent<Bullet>().TimeToLive = BulletTimeToLive;
            thebullet.GetComponent<Bullet>().LifeSteal = LifeSteal;
            thebullet.GetComponent<Bullet>().WhatToHit = WhatToHit;
            thebullet.GetComponent<Bullet>().BulletSpeed = BulletSpeed;
            thebullet.GetComponent<Bullet>().Damage = Damage;
        }
    }

    ///The ShootFullAuto function allows the gun to shoot FullAuto.
    /**The function checks if the player is shooting at full auto and if Ammo amount is above 0.
    If true, it gathers information about the current tokens lifesteal stats, subtracts one from the ammo value, generates a random vertical spread,
    plays a muzzleflash animation (light at the end of the gun when shooting), instantiates a bullet gameobject based on a prefab, and set its correct location to be at the end of the gunbarrel.
    then it sets the rotation of the bullet to be equal the _randomSpread and lastly assigns necessary values to the bullet 
    (e.g. lifesteal, camerashake strength, what it can hit, bulletspeed and damage).*/

    /*!The function then starts a new coroutine where the ShootFullAuto function is called with a delay equal to the firerate.*/
    void ShootFullAuto()
    {
        if (IsShootingFullAuto == true)
        {
            if (Ammo > 0)
            {
                LifeSteal = TokenManagerScript.GunLifeStealAmount;

                MuzzleFlashAnimator.Play(MuzzleFlashAnimationName);
                Ammo -= 1;
                float _randomSpread = Random.Range(MinVerticalSpread, MaxVerticalSpread);
                GameObject thebullet = Instantiate(BulletObject, FirePoint.position, FirePoint.rotation);
                thebullet.transform.Rotate(0, 0, _randomSpread);
                thebullet.GetComponent<Bullet>().CameraShakeStrength = CameraShakeStrength;
                thebullet.GetComponent<Bullet>().WhatToHit = WhatToHit;
                thebullet.GetComponent<Bullet>().TimeToLive = 1f;
                thebullet.GetComponent<Bullet>().BulletSpeed = BulletSpeed;
                thebullet.GetComponent<Bullet>().LifeSteal = LifeSteal;
                thebullet.GetComponent<Bullet>().Damage = Damage;
                thebullet.GetComponent<Rigidbody2D>().mass = BulletWeight;

            }
            StartCoroutine(FullAutoCooldown());
        }
    }
    ///An IEnumerator that calls ShootFullAuto after a delay equal to the firerate has passed.
    IEnumerator FullAutoCooldown()
    {
        FirerateCounter = Firerate;
        yield return new WaitForSeconds(Firerate);
        ShootFullAuto();
    }

}
