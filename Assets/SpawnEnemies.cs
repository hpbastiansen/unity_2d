using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _enemies;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform _spawnPoint in _spawnPoints)
        {
            GameObject _enemyToSpawn = _enemies[Random.Range(0, _enemies.Length)];
            Instantiate(_enemyToSpawn, _spawnPoint.position, Quaternion.identity);
        }
    }
}
