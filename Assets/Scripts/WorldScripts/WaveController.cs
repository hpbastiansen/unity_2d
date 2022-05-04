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
    private List<GameObject> _enemiesToEnable;

    private bool _hasBeenTriggered = false;

    private int _currentWave = 0;


    void Start()
    {
        _enemiesToEnable = new List<GameObject>();
    }
    public void Trigger()
    {
        if (_hasBeenTriggered) return;
        _hasBeenTriggered = true;
        Invoke("NextWave", _timeToFirstWave);
    }

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

    void EnableAI()
    {
        if(_enemiesToEnable[0] != null) _enemiesToEnable[0].GetComponent<EnemyAI_Hunter>().enabled = true;
        _enemiesToEnable.RemoveAt(0);
    }
}
