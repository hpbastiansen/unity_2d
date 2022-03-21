using System.Collections.Generic;
using UnityEngine;

///The WormSpawner script randomly spawns enemy worms around the player if certain conditions are met.
public class WormSpawner : MonoBehaviour
{
    [Header("Spawn range")]
    [SerializeField] private float _maxSpawnRange = 15f;
    [SerializeField] private float _minSpawnRange = 4f;
    [SerializeField] private int _maxWorms = 2;

    [Header("Spawn time")]
    [SerializeField] private float _spawnTimeMin = 5f;
    [SerializeField] private float _spawnTimeMax = 10f;

    [Header("Objects")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _worm;

    private float _timeSinceSpawn = 0f;
    private float _lastCheck = 0f;
    private GameObject[] _wormPoints;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*!In the Start function all spawn points in the level are identified.*/
    private void Start()
    {
        _wormPoints = GameObject.FindGameObjectsWithTag("WormSpawn");
    }

    ///Update is called every frame
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often*/
    /*! In the update function we add the time elapsed since the last frame, and check if spawn conditions are met every second. */
    private void Update()
    {
        // Increase by time elapsed since last frame.
        _timeSinceSpawn += Time.deltaTime;
        _lastCheck += Time.deltaTime;

        if(_lastCheck > 1f)
        {
            _lastCheck = 0f;
            CheckShouldSpawn();
        }
    }

    ///Checks if spawn conditions are met, and if so - spawns a worm.
    /**This function checks if the minimum spawn time has elapsed, if the max amount of worms is not reached and if there are spawn points nearby. 
     * 
     * The spawn chance is normalized with _spawnTimeMin as 0, _spawnTimeMax as 1. 
     * A random number between 0 and 1 is generated - if it's lower than the spawn chance number, a worm is spawned. */
    private void CheckShouldSpawn()
    {
        if (_timeSinceSpawn < _spawnTimeMin) return;
        if (GameObject.FindGameObjectsWithTag("Worm").Length > _maxWorms) return;

        List<Vector2> _selectedPoints = SelectPoints();
        if (_selectedPoints.Count < 3) return;

        float _spawnChance = (_timeSinceSpawn - _spawnTimeMin) / (_spawnTimeMax - _spawnTimeMin);
        if (Random.value < _spawnChance) SpawnWorm(_selectedPoints);
    }

    ///Selects spawn points on each side of the player.
    /**This function selects a random spawn point on each side of the player, and returns a list with these points and a point on the players position.
     * The spawn point of the worm is chosen as the point farthest away from the player, and is put as the first point in the list.*/
    private List<Vector2> SelectPoints()
    {
        List<Vector2> _chosenPoints = new List<Vector2>();
        List<Vector2> _leftPoints = new List<Vector2>();
        List<Vector2> _rightPoints = new List<Vector2>();

        // Check if a spawn point is within the spawn range, and add it to the corresponding list.
        foreach (GameObject _point in _wormPoints)
        {
            float _distance = Vector3.Distance(_point.transform.position, _player.transform.position);
            if(_distance < _maxSpawnRange && _distance > _minSpawnRange)
            {
                if (_point.transform.position.x < _player.transform.position.x)
                {
                    _leftPoints.Add(_point.transform.position);
                }
                else
                {
                    _rightPoints.Add(_point.transform.position);
                }
            }
        }

        // If no valid points are found either to the left or right, return an empty list.
        if (_leftPoints.Count == 0 || _rightPoints.Count == 0) return new List<Vector2>();

        // Select a random point to the left and right.
        Vector2 _leftPoint = _leftPoints[(int)Mathf.Floor(Random.Range(0f, _leftPoints.Count))];
        Vector2 _rightPoint = _rightPoints[(int)Mathf.Floor(Random.Range(0f, _rightPoints.Count))];

        // Add the furthest point into the list first. This point will be the spawn point of the worm.
        Vector2 _playerPoint = new Vector2(_player.transform.position.x, _player.transform.position.y + 0.2f);
        if(Vector2.Distance(_leftPoint, _playerPoint) > Vector2.Distance(_rightPoint, _playerPoint))
        {
            _chosenPoints.Add(_leftPoint);
            _chosenPoints.Add(_playerPoint);
            _chosenPoints.Add(_rightPoint);
        } else
        {
            _chosenPoints.Add(_rightPoint);
            _chosenPoints.Add(_playerPoint);
            _chosenPoints.Add(_leftPoint);
        }

        return _chosenPoints;
    }

    ///Spawns a worm into the scene.
    /**Instantiates a worm into the scene and sets the start, end, and player positions. */
    private void SpawnWorm(List<Vector2> _points)
    {
        _timeSinceSpawn = 0f;

        // Instantiate worm and set path points.
        GameObject _spawnedWorm = Instantiate(_worm, _points[0], Quaternion.Euler(0, 0, 0));
        WormPath _path = _spawnedWorm.GetComponent<WormPath>();
        _path.StartPoint = _points[0];
        _path.PlayerPoint = _points[1];
        _path.EndPoint = _points[2];
        _path.Init();
    }
}
