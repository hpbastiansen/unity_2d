using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawnerBoss : MonoBehaviour
{
    private Vector3 _pointTo;
    private float _timeSinceSpawn = 0;
    private float _lastCheck = 0;
    private GameObject _currentParticles;

    [Header("Angle")]
    [SerializeField] private float _angleMin;
    [SerializeField] private float _angleMax;

    [Header("Spawn time")]
    [SerializeField] private float _spawnTimeMin;
    [SerializeField] private float _spawnTimeMax;

    [Header("Other")]
    [SerializeField] private GameObject _worm;
    [SerializeField] private GameObject _particles;

    // Update is called once per frame
    void Update()
    {
        _timeSinceSpawn += Time.deltaTime;
        _lastCheck += Time.deltaTime;

        if (_lastCheck > 1f)
        {
            _lastCheck = 0f;
            float _spawnChance = (_timeSinceSpawn - _spawnTimeMin) / (_spawnTimeMax - _spawnTimeMin);
            if (Random.value < _spawnChance) SpawnWorm();
        }
    }

    void SpawnWorm()
    {
        _timeSinceSpawn = 0f;
        

        StartCoroutine(Spawn());
        //Invoke("Spawn", 1f);
    }

    IEnumerator Spawn()
    {
        _currentParticles = Instantiate(_particles, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(_currentParticles);

        float _angle = Random.Range(_angleMin, _angleMax) * Mathf.Deg2Rad;
        Vector3 _direction = new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0);
        _pointTo = transform.position + (_direction * 50f);

        GameObject _spawnedWorm = Instantiate(_worm, transform.position, Quaternion.Euler(0, 0, 0));
        WormPathSimple _path = _spawnedWorm.GetComponent<WormPathSimple>();
        _path.StartPoint = transform.position;
        _path.EndPoint = _pointTo;
        _path.Init();
    }
}
