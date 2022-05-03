using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpoint : MonoBehaviour
{
    [SerializeField] private float _health;

    void Update()
    {
        if (_health <= 0)
        {
            GameObject.Find("BossController").GetComponent<BossController>().WeakpointDestroyed();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        _health = _health - dmg;
    }
}
