using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private float _timeToFirstWave;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private int[] _amounts;
    [SerializeField] private float[] _timesToNextWave;
    [SerializeField] Transform[] _spawnPoints;

    private bool _hasBeenTriggered = false;

    private int _currentWave = 0;


    void Start()
    {
        Trigger();
    }
    void Trigger()
    {
        if (_hasBeenTriggered) return;
        _hasBeenTriggered = true;
        Invoke("NextWave", _timeToFirstWave);
    }

    void NextWave()
    {
        if (_currentWave >= _enemies.Length) return;

        for(int _i = 0; _i < _amounts[_currentWave]; _i++)
        {
            int _randomSpawnPoint = Random.Range(0, _spawnPoints.Length);
            Transform _spawnPoint = _spawnPoints[_randomSpawnPoint];
            Instantiate(_enemies[_currentWave], _spawnPoint.position, Quaternion.identity);
        }

        Invoke("NextWave", _timesToNextWave[_currentWave]);
        _currentWave++;
    }
}
