using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private bool _canGrappleThrough;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!_canGrappleThrough || !_player.GetComponent<GrapplingHookController>().IsHooked)
            {
                _player.GetComponent<PlayerHealth>().TakeDamage(9999, 0);
            }
        }
    }
}
