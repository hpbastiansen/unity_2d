using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GameObject _player;
    public string KeyName;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == _player)
        {
            _player.GetComponent<PlayerKeys>().AddKey(KeyName);
            Destroy(gameObject);
        }
    }
}
