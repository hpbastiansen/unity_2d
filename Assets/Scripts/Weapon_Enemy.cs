using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Weapon_Enemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    public float speedofbullet = 1f;
    public float minSpreadY = 0f;
    public float maxSpreadY = 0f;

    public int Ammo = 1;
    public bool maxAmmo;
    public float firerate;
    public float fireratecounter = 0;

    public float camshakestrength = 1;
    public CinemachineImpulseListener camscript;
    public CinemachineVirtualCamera cam;
    public Animator ShootlightAnim;
    public string nameofanim;
    public LayerMask whattohit;

    private void Start()
    {
        ShootFullAuto();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (maxAmmo == true)
        {
            Ammo = 100000;
        }

        if (fireratecounter >= 0)
        {
            fireratecounter -= Time.deltaTime;
        }
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
    IEnumerator FullAutoCooldown()
    {
        fireratecounter = firerate;
        yield return new WaitForSeconds(firerate);
        ShootFullAuto();
    }

}
