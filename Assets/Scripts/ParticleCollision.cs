using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public CinemachineVirtualCamera cam;
    public GameObject explosionPrefab;
    public WeaponTemplate weaponTemplate;
    private float weapondamage;

    public GameObject explotionPrefab;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        weaponTemplate = GetComponentInParent<WeaponTemplate>();
        weapondamage = weaponTemplate.Damage;
    }
    void Update()
    {
        weapondamage = weaponTemplate.Damage;
    }
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);

        ParticleSystem p = explosion.GetComponent<ParticleSystem>();
        var pmain = p.main;

        cam.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        EnemyTemplate et = other.GetComponent<EnemyTemplate>();
        if (et)
        {
            Debug.Log("Hit an enemy");
            et.Takedmg(weapondamage);
        }


    }
}
