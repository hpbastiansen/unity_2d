using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHP : MonoBehaviour
{
    public float HP;
    public float maxHP;
    public float rechargeTimer;
    public WeaponController wc;
    public PlayerHealth playerHP;
    public float lifeSteal;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        wc = GameObject.Find("Main_Character").GetComponent<WeaponController>();
        playerHP = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDmg(float x)
    {
        HP -= x;
        if (HP <= 0)
        {
            Debug.Log("SHIELD DOWN!");
            wc.usingShield = !wc.usingShield;
            wc.shieldRechargeTimer = rechargeTimer;
        }
        LifeSteal(x, lifeSteal);

    }
    public void LifeSteal(float x, float y)
    {
        playerHP.HP += x / y;
    }
}
