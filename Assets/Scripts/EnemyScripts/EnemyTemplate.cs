using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///The EnemyTemplate script is a template for health system and ability to take damage for enemies.
public class EnemyTemplate : MonoBehaviour
{

    public float HP;
    public Slider Healthbar;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*!In the Start function the healthbar is given its health value.*/
    void Start()
    {
        Healthbar = transform.Find("Canvas").transform.Find("HealthBar").GetComponent<Slider>();
        Healthbar.maxValue = HP;
    }

    ///Update is called every frame
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often*/
    /*! In the Update function we contantly set the healthbar to be equal the current health. So whenever the TakeDamage function is called and a change in health occurs the healthbar will be updated. 
    Lastly we check if the Health is below or equals zero, and if it does, we destroy the given gameobject.*/
    void Update()
    {
        Healthbar.value = HP;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    ///TakeDamage removes given damage from health.
    /**This function can be called from anywhere (e.g a bullet hit script) and by supplying a damage value in its parameter we can easily remove that amount from the enemies' health value.*/
    public void TakeDamage(float dmg)
    {
        HP = HP - dmg;
    }
}
