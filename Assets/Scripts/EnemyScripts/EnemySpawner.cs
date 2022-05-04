using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _enemiesAmt;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _enemyDistance;

    // Start is called before the first frame update
    void Start()
    {
        for(int _i = 1; _i <= _enemiesAmt; _i++)
        {
            int _enemyType = Random.Range(0, _enemies.Length);
            float _distance = _enemyDistance * (_i % 2 == 0 ? -1 * _i : _i);
            GameObject _spawned = Instantiate(_enemies[_enemyType], new Vector3(transform.position.x + _distance, transform.position.y), Quaternion.identity);
            _spawned.GetComponent<EnemyAI>().EnemyPoint = gameObject;
        }
    }
}
