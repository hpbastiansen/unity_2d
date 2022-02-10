using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    public float speedofbullet = 1f;
    public float minSpreadY = 0f;
    public float maxSpreadY = 0f;

    public int Ammo = 1;
    public bool FullAuto;
    public bool isShootingfullauto;
    public float firerate;
    public float fireratecounter = 0;

    public float camshakestrength = 1;
    public CinemachineImpulseListener camscript;
    public CinemachineVirtualCamera cam;
    public Animator ShootlightAnim;
    public string nameofanim;
    public bool isPlayer;
    public LayerMask whattohit;


    // Update is called once per frame
    void LateUpdate()
    {
        if (FullAuto == true)
        {
            if (Input.GetMouseButton(0) && Time.time >= fireratecounter)
            {
                fireratecounter = Time.time + 1f / firerate;
                Shoot();
            }
        }
        else if (FullAuto == false)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= fireratecounter)
            {
                fireratecounter = Time.time + 1f / firerate;
                Shoot();
            }
        }
    }
    private void OnEnable()
    {
        isShootingfullauto = false;
    }
    void Shoot()
    {
        if (Ammo > 0)
        {
            ShootlightAnim.Play(nameofanim);
            Ammo -= 1;
            float randomspread = Random.Range(minSpreadY, maxSpreadY);
            GameObject thebullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            thebullet.transform.Rotate(0, 0, randomspread);
            thebullet.GetComponent<Bullet>().camshakestrength = camshakestrength;
            thebullet.GetComponent<Bullet>().cam = cam;
            thebullet.GetComponent<Bullet>().camscript = camscript;
            thebullet.GetComponent<Bullet>().whattohit = whattohit;
            thebullet.GetComponent<Bullet>().speed = speedofbullet;
        }
    }
    void ShootFullAuto()
    {
        if (isShootingfullauto == true)
        {
            if (Ammo > 0)
            {
                ShootlightAnim.Play(nameofanim);
                Ammo -= 1;
                float randomspread = Random.Range(minSpreadY, maxSpreadY);
                GameObject thebullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                thebullet.transform.Rotate(0, 0, randomspread);
                thebullet.GetComponent<Bullet>().camshakestrength = camshakestrength;
                thebullet.GetComponent<Bullet>().cam = cam;
                thebullet.GetComponent<Bullet>().camscript = camscript;
                thebullet.GetComponent<Bullet>().whattohit = whattohit;
                thebullet.GetComponent<Bullet>().speed = speedofbullet;
            }
            StartCoroutine(FullAutoCooldown());
        }
    }
    IEnumerator FullAutoCooldown()
    {
        fireratecounter = firerate;
        yield return new WaitForSeconds(firerate);
        ShootFullAuto();
    }

}
