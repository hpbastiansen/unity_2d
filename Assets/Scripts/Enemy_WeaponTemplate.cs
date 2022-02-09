using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Enemy_WeaponTemplate : MonoBehaviour
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
    private bool isShootingFullAuto;
    private ParticleSystem ps;
    private Rigidbody2D rb;
    [Header("Camera Shake Strength")]
    public float camshakestrength = 1;
    public CinemachineImpulseListener camscript;
    ParticleSystem.VelocityOverLifetimeModule vsaa;
    public Animator ShootlightAnim;
    public string nameofanim;



    // Start is called before the first frame update
    void Start()
    {
        isShootingFullAuto = false;
        ps = gameObject.transform.Find("ShootParticles").GetComponent<ParticleSystem>();
        vsaa = ps.velocityOverLifetime;
        SetVel(SpreadYmin, SpreadYmax);
        rb = GameObject.Find("Enemy").GetComponent<Rigidbody2D>();
        ShootFullAuto();
    }

    // Update is called once per frame
    void Update()
    {
        //camscript.m_Gain = camshakestrength;

    }

    void ShootFullAuto()
    {
        isShootingFullAuto = true;
        gun.Play();
        Knockback(knockback);
        ShootlightAnim.Play(nameofanim);
        StartCoroutine(FullAutoShotCooldown());
    }
    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(firerate);
    }
    IEnumerator FullAutoShotCooldown()
    {
        yield return new WaitForSeconds(firerate);
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
}
