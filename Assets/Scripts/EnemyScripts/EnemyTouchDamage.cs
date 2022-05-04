using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] float _damage = 20f;
    [SerializeField] float _knockback = 10f;
    private float _angle;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == _player)
        {
            float _deltaX = transform.position.x - _player.transform.position.x;
            _angle = _deltaX > 0 ? 135f : 45f;

            _player.GetComponent<PlayerHealth>().TakeDamage(_damage, _knockback, _angle);
        }
    }
}
