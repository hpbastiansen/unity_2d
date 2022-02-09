using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class Enemy_ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public CinemachineVirtualCamera cam;
    public GameObject explosionPrefab;
    public GameObject blockPrefab;

    public Enemy_WeaponTemplate weaponTemplate;
    private int weapondamage;

    public GameObject explotionPrefab;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        weaponTemplate = GetComponentInParent<Enemy_WeaponTemplate>();
    }
    void Update()
    {
        //weapondamage = weaponTemplate.Damage;

    }
    void OnParticleCollision(GameObject other)
    {
        PlayerHealth ph;
        ph = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);


        if (ph.isBlocked == false)
        {
            GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);
            ParticleSystem p = explosion.GetComponent<ParticleSystem>();
            var pmain = p.main;
        }
        else if (ph.isBlocked == true)
        {
            GameObject block = Instantiate(blockPrefab, collisionEvents[0].intersection, Quaternion.identity);
            ParticleSystem p = block.GetComponent<ParticleSystem>();
            var pmain = p.main;

        }


        cam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        if (other.gameObject.tag == "Player" && other.GetComponent<PlayerHealth>())
        {
            Debug.Log("Hit player");
            ph.TakeDmg(weapondamage);
        }
        if (other.gameObject.tag == "Shield")
        {
            Debug.Log("Hit shield");
        }


    }
}
