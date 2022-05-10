using UnityEngine;

/// This script spawns enemies around an enemy spawnpoint.
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _enemiesAmt;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _enemyDistance;

    /// Called before the first frame.
    /** For every enemy to spawn, we choose a random enemy type from the _enemies array. The distance to spawn the enemies is calculated, 
        making sure the enemies do not spawn on top of each other, but around the spawnpoint. Lastly we instantiate the enemy and set it's spawn point as this gameobject. */
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
