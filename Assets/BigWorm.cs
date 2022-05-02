using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWorm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ScreenShakeController.Instance.StartShake(1f, 0.07f, true);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(0, 40, 135);
        }
    }
}
