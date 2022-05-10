using UnityEngine;

/// This script spawns a random enemy on each of the spawn points connected to this script.
public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _enemies;

    /// Called before the first frame.
    /** In the Start method, for every spawnpoint, we choose a random enemy from the _enemies array and instantiate it on the point. */
    void Start()
    {
        foreach(Transform _spawnPoint in _spawnPoints)
        {
            GameObject _enemyToSpawn = _enemies[Random.Range(0, _enemies.Length)];
            Instantiate(_enemyToSpawn, _spawnPoint.position, Quaternion.identity);
        }
    }
}
