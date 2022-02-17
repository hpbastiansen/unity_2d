using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponController : MonoBehaviour
{
    int totalWeapons = 1;
    public int currentWeaponIndex;

    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;
    public bool usingShield;
    public GameObject shield;

    public float shieldRechargeTimer;


    // Start is called before the first frame update
    void Start()
    {
        usingShield = false;
        totalWeapons = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapons];

        for (int i = 0; i < totalWeapons; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }

        guns[0].SetActive(true);
        currentGun = guns[0];
        currentWeaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /* 
        NOT IN USE AT THE TIME. SOURCE IF NEEDED FOR FUTURE.
        ////////////////////////////////////////////////////////////////
        https://www.youtube.com/watch?v=-YISSX16NwE&ab_channel=TheGameGuy
        ////////////////////////////////////////////////////////////////
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //next Weapon
                    if (currentWeaponIndex < totalWeapons - 1)
                    {
                        guns[currentWeaponIndex].SetActive(false);
                        currentWeaponIndex += 1;
                        guns[currentWeaponIndex].SetActive(true);
                        currentGun = guns[currentWeaponIndex];
                    }
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    //previous Weapon
                    if (currentWeaponIndex > 0)
                    {
                        guns[currentWeaponIndex].SetActive(false);
                        currentWeaponIndex -= 1;
                        guns[currentWeaponIndex].SetActive(true);
                        currentGun = guns[currentWeaponIndex];
                    }
                }*/
        if (Input.GetMouseButtonDown(2) && shieldRechargeTimer <= 0)
        {
            usingShield = !usingShield;
        }
        if (usingShield == true)
        {
            shield.SetActive(true);
            weaponHolder.SetActive(false);
        }
        else if (usingShield == false)
        {
            shield.SetActive(false);
            weaponHolder.SetActive(true);
        }
        if (shieldRechargeTimer > 0)
        {
            shieldRechargeTimer -= Time.deltaTime;
            shield.GetComponent<ShieldHP>().HP = shield.GetComponent<ShieldHP>().maxHP;

        }
    }
}
