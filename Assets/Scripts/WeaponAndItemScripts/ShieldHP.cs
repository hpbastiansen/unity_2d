using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///The SheildHP script's purpose is to keep track of key features for the sheild like HP, RechargeTimer, LifeStealAmount and more.
public class ShieldHP : MonoBehaviour
{
    public float HP;
    public float MaxHP;
    public float RechargeTimer;
    public WeaponController WeaponController;
    public PlayerHealth PlayerHealth;
    public float LifeStealAmount;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function the HP is set and the WeaponController and PlayerHealth is located and assigned.*/
    void Start()
    {
        HP = MaxHP;
        WeaponController = GameObject.Find("Main_Character").GetComponent<WeaponController>();
        PlayerHealth = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();
    }

    ///The TakeDamage function allows any object in the game to take damage on the shield and decrease the health.
    /**If the HP of the shield is equal or below zero the shield is disabled and the recharge timer is set to given value.
    For each hit the shield takes the LifeSteal function is called.
    */
    public void TakeDamage(float _damage)
    {
        HP -= _damage;
        if (HP <= 0)
        {
            Debug.Log("SHIELD DOWN!");
            WeaponController.UsingShield = !WeaponController.UsingShield;
            WeaponController.ShieldRechargeTimer = RechargeTimer;
        }
        LifeSteal(_damage, LifeStealAmount);
    }

    ///The LifeSteal function allows the shield to absorb some of the damge taken and give it to the players health.
    /**The amount of HP given to the player is calculated by taking the input damage and divide it by the _lifeStealAmount.
    Meaning the higher value is, the lower amounts of HP is given to the player.*/
    public void LifeSteal(float _damage, float _lifeStealAmount)
    {
        PlayerHealth.HP += _damage / _lifeStealAmount;
    }
}
