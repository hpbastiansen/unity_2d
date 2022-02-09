using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponTemplate : MonoBehaviour
{
    [Header("Weapon attributes")]
    public float firerate = 1;
    public float Damage = 1;
    public float SpreadYmin;
    public float SpreadYmax;
    public float knockback;
    public int ammo = 100;
    public ParticleSystem gun;
    public bool isFullAuto;
    public bool canShoot;
    private bool isShootingFullAuto;
    private ParticleSystem ps;
    private Rigidbody2D rb;
    public Transform firepoint;
    [Header("Camera Shake Strength")]
    public float camshakestrength = 1;
    public CinemachineImpulseListener camscript;
    ParticleSystem.VelocityOverLifetimeModule vsaa;
    public Animator ShootlightAnim;
    public string nameofanim;



    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        isShootingFullAuto = false;
        ps = gameObject.transform.Find("ShootParticles").GetComponent<ParticleSystem>();
        vsaa = ps.velocityOverLifetime;
        SetVel(SpreadYmin, SpreadYmax);
        rb = GameObject.Find("Main_Character").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        camscript.m_Gain = camshakestrength;
        var psemission = ps.emission;
        psemission.rateOverTime = firerate;
        if (isFullAuto == false)
        {
            if (Input.GetMouseButtonDown(0) && canShoot)
            {
                Shoot();
            }
        }
        if (isFullAuto == true)
        {
            if (Input.GetMouseButton(0))
            {
                isShootingFullAuto = true;
                gun.Play();
                //ShootFullAuto();

            }
            if (Input.GetMouseButtonUp(0))
            {
                isShootingFullAuto = false;
                gun.Stop();

            }

        }
    }

    void Shoot()
    {
        canShoot = false;
        gun.Play();
        Knockback(knockback);
        ShootlightAnim.Play(nameofanim);
        StartCoroutine(ShotCooldown());

    }
    void ShootFullAuto()
    {
        //canShoot = false;
        gun.Play();
        Knockback(knockback);
        ShootlightAnim.Play(nameofanim);
        StartCoroutine(FullAutoShotCooldown());
    }
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(firerate);
        canShoot = true;
    }
    IEnumerator FullAutoShotCooldown()
    {
        yield return new WaitForSeconds(firerate);
        //canShoot = true;
        if (isShootingFullAuto == true)
        {
            ShootFullAuto();
        }
    }
    void Knockback(float x)
    {
        rb.AddForce(transform.right * -knockback, ForceMode2D.Force);
    }
    void SetVel(float xmin, float xmax)
    {
        vsaa.y = new ParticleSystem.MinMaxCurve(xmax, xmin);
    }
    public void allowShoot()
    {
        canShoot = true;
    }
}
