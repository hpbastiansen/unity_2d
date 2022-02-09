using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyTemplate : MonoBehaviour
{

    public float HP;
    public Slider Healthbar;

    // Start is called before the first frame update
    void Start()
    {
        Healthbar = GameObject.Find("HealthBar").GetComponent<Slider>();
        Healthbar.maxValue = HP;
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = HP;
        if (HP <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy died");
        }

    }

    public void Takedmg(float dmg)
    {
        HP = HP - dmg;
    }
}
