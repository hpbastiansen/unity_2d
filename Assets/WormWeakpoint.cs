using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormWeakpoint : MonoBehaviour
{
    [SerializeField] private float _health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
            // Trigger cutscene with worm
        }
    }

    public void TakeDamage(float dmg)
    {
            _health = _health - dmg;
    }
}
