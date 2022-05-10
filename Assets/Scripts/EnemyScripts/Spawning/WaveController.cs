using System.Collections.Generic;
using UnityEngine;

/// This script controls waves of enemies to spawn at certain spawnpoints.
public class WaveController : MonoBehaviour
{
    [SerializeField] private float _timeToFirstWave;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private int[] _amounts;
    [SerializeField] private float[] _timesToNextWave;
    [SerializeField] Transform[] _spawnPoints;
    private List<GameObject> _enemiesToEnable = new List<GameObject>();

    private bool _hasBeenTriggered = false;

    private int _currentWave = 0;

    /// This method triggers the Wave Controller, spawning the first wave after a specified amount of seconds.
    public void Trigger()
    {
        if (_hasBeenTriggered) return;
        _hasBeenTriggered = true;
        Invoke("NextWave", _timeToFirstWave);
    }

    /// The NextWave method is responsible for spawning the next wave of enemies.
    /** If there is no next wave, the method returns, which stops the Wave Controller. 
        For every enemy to spawn, we select a random spawn point. We instantiate the enemy and turn off their AI component.
        After a specified offset has passed, we turn the AI on again. 
        Afterwards, we invoke the next wave after the specified amount of seconds. */
    void NextWave()
    {
        if (_currentWave >= _enemies.Length) return;

        float _offset = 0.5f;

        for(int _i = 0; _i < _amounts[_currentWave]; _i++)
        {
            int _randomSpawnPoint = Random.Range(0, _spawnPoints.Length);
            Transform _spawnPoint = _spawnPoints[_randomSpawnPoint];
            GameObject _spawnedEnemy = Instantiate(_enemies[_currentWave], _spawnPoint.position, Quaternion.identity);
            _enemiesToEnable.Add(_spawnedEnemy);
            _spawnedEnemy.GetComponent<EnemyAI_Hunter>().enabled = false;
            Invoke("EnableAI", _offset);
            _offset += 0.3f;
        }

        Invoke("NextWave", _timesToNextWave[_currentWave]);
        _currentWave++;
    }

    /// This method Enables the AI component which is turned off in the NextWave method.
    void EnableAI()
    {
        if(_enemiesToEnable[0] != null) _enemiesToEnable[0].GetComponent<EnemyAI_Hunter>().enabled = true;
        _enemiesToEnable.RemoveAt(0);
    }
}
