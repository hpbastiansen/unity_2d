using UnityEngine;

/// This script randomly spawns worms in the world, going in a straight line from one point to another.
public class WormSpawnerSimple : MonoBehaviour
{
    [Header("Spawn points")]
    [SerializeField] private Transform[] _pointsFrom;
    [SerializeField] private Transform[] _pointsTo;

    [Header("Spawn time")]
    [SerializeField] private float _spawnTimeMin = 5f;
    [SerializeField] private float _spawnTimeMax = 10f;

    [Header("Other")]
    [SerializeField] private GameObject _worm;

    private float _timeSinceSpawn;
    private float _lastCheck = 0f;

    /// Called every frame.
    /** If one second has elapsed, check if we should spawn a worm. 
        The spawn chance is normalized with _spawnTimeMin as 0, _spawnTimeMax as 1. 
        A random number between 0 and 1 is generated - if it's lower than the spawn chance number, a worm is spawned. */
    void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
        _lastCheck += Time.deltaTime;

        if(_lastCheck > 1f)
        {
            _lastCheck = 0f;
            float _spawnChance = (_timeSinceSpawn - _spawnTimeMin) / (_spawnTimeMax - _spawnTimeMin);
            if (Random.value < _spawnChance) SpawnWorm();
        }        
    }

    /// Select a random spawnpoint from the _pointsFrom array, and a random endpoint from the _poinsTo array. Instantiate a worm and pass the points to its WormPathSimple script.
    void SpawnWorm()
    {
        _timeSinceSpawn = 0f;

        Transform _pointFrom = _pointsFrom[Random.Range(0, _pointsFrom.Length)];
        Transform _pointTo = _pointsTo[Random.Range(0, _pointsFrom.Length)];

        GameObject _spawnedWorm = Instantiate(_worm, _pointFrom.position, Quaternion.Euler(0, 0, 0));
        WormPathSimple _path = _spawnedWorm.GetComponent<WormPathSimple>();
        _path.StartPoint = _pointFrom.position;
        _path.EndPoint = _pointTo.position;
        _path.Init();
    }
}
