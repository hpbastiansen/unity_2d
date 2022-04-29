using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] float _damage = 20f;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == _player)
        {
            _player.GetComponent<PlayerHealth>().TakeDamage(_damage);
        }
    }
}
