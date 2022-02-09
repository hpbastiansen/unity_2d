using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 10;
    public GameObject hitEffect;
    public GameObject blockEffect;

    public CinemachineVirtualCamera cam;
    public CinemachineImpulseListener camscript;
    public LayerMask whattohit;
    public float camshakestrength = 1;

    public bool enemyBullet;

    void Start()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(kill());
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if ((whattohit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) //http://answers.unity.com/answers/1394106/view.html
        {
            EnemyTemplate enemy = other.gameObject.GetComponent<EnemyTemplate>();
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (enemy != null)
            {
                enemy.Takedmg(damage);
                Instantiate(hitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                ScreenShakeController.instance.StartShake(.05f, .03f);
                Destroy(gameObject);
            }
            else if (player != null)
            {
                player.TakeDmg(damage);
                if (player.isBlocked == true)
                {
                    Instantiate(blockEffect, transform.position, blockEffect.transform.rotation);
                    Destroy(gameObject);
                }
                else
                {
                    Instantiate(hitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                    ScreenShakeController.instance.StartShake(.2f, .1f);
                    Destroy(gameObject);
                }
            }
            else
            {
                Instantiate(hitEffect, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
                ScreenShakeController.instance.StartShake(.05f, .03f);
                Destroy(gameObject);
            }

        }
    }
    IEnumerator kill()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}