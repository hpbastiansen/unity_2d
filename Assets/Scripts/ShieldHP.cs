using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHP : MonoBehaviour
{
    public float HP;
    public float maxHP;
    public float rechargeTimer;
    public WeaponController wc;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        wc = GameObject.Find("Main_Character").GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDmg(int x)
    {
        HP -= x;
        if (HP <= 0)
        {
            Debug.Log("SHIELD DOWN!");
            wc.usingShield = !wc.usingShield;
            wc.shieldRechargeTimer = rechargeTimer;
        }

    }
}
