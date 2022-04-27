using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _key;
    private string _keyName;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _keyName = _key.GetComponent<Key>().KeyName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            if (_player.GetComponent<PlayerKeys>().HasKey(_keyName))
            {
                _collider.enabled = false;
                _spriteRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == _player)
        {
            _collider.enabled = true;
            _spriteRenderer.enabled = true;
        }
    }
}
