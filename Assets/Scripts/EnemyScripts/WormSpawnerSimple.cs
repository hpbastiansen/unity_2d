using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
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
