using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    enum AIPhase { Undetected, Pursuit, Searching, Alert };
    enum Direction { Left, Right };
    private AIPhase _currentPhase = AIPhase.Undetected;
    private Direction _currentDirection = Direction.Left;

    [HideInInspector] public GameObject EnemyPoint;
    private Seeker _seeker;
    private EnemyMovement _enemyMovement;
    private Weapon_Enemy _enemyWeapon;

    private GameObject _player;
    private float _distanceToPlayer;

    private Path _path;
    private bool _reachedEndOfPath = false;
    private int _currentWaypoint = 0;

    private bool _isCreatingPath = false;

    private Vector3 _searchPosition;

    [Header("Pathfinding settings")]
    [SerializeField] private float _nextWaypointDistance = 1f;

    [Header("Patrol radius")]
    [SerializeField] private float _radiusUndetected = 10f;
    [SerializeField] private float _radiusAlert = 20f;
    [SerializeField] private float _radiusSearch = 20f;

    [Header("Line of sight settings")]
    [SerializeField] private float _lineOfSightUndetected = 5f;
    [SerializeField] private float _lineOfSightAlert = 10f;
    [SerializeField] private float _detectionTimeUndetected = 2f;
    [SerializeField] private float _detectionTimeAlert = 1f;
    [SerializeField] private float _pursuitTime = 4f;
    [SerializeField] private float _searchTime = 10f;
    [SerializeField] private LayerMask _rayColliders;
    private float _lineOfSightTimer = 0f;
    [SerializeField] private float _shootRange = 5f;

    [Header("Walking length")]
    [SerializeField] private float _minWalkLength = 2f;
    [SerializeField] private float _maxWalkLength = 5f;

    [Header("Waiting time")]
    private float _waitTimer = 0f;
    private float _waitLength;
    [SerializeField] private float _minWaitLength = 3f;
    [SerializeField] private float _maxWaitLength = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<Weapon_Enemy>();
        _seeker = GetComponent<Seeker>();
        _seeker.pathCallback += OnPathComplete;
        _waitLength = Random.Range(_minWaitLength, _maxWaitLength);
        GenerateWalkingPath();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLineOfSight();
        if (_currentPhase == AIPhase.Undetected)
        {
            _enemyMovement.Running = false;
            if (_reachedEndOfPath)
            {
                _waitTimer += Time.deltaTime;
                if(_waitTimer > _waitLength)
                {
                    GenerateWalkingPath();
                    _waitLength = Random.Range(_minWaitLength, _maxWaitLength);
                    _waitTimer = 0f;
                }
            }
            if (_path == null) return;

            FollowPath();
        }
        else if (_currentPhase == AIPhase.Pursuit)
        {
            _enemyMovement.Running = true;
            if(_reachedEndOfPath) return;

            if (_distanceToPlayer > _shootRange) FollowPath();
            else return;
            
        }
        else if (_currentPhase == AIPhase.Searching)
        {
            // Generate random paths around the players last known position
            _enemyMovement.Running = false;
            if (_reachedEndOfPath) GenerateSearchPath();
            if (_path == null) return;

            FollowPath();
        }
        else if (_currentPhase == AIPhase.Alert)
        {
            _enemyMovement.Running = true;
            if(_reachedEndOfPath)
            {
                _waitTimer += Time.deltaTime;
                if(_waitTimer > _waitLength)
                {
                    GenerateAlertPath();
                    _waitLength = Random.Range(_minWaitLength, _maxWaitLength);
                    _waitTimer = 0f;
                }
            }
            if (_path == null) return;

            FollowPath();
            // Go back home. Run around spawn point.
        }
    }

    private void FollowPath()
    {
        Vector3 _nextWaypoint = _path.vectorPath[_currentWaypoint];
        if (transform.position.x - _nextWaypoint.x > 0.4f)
        {
            _enemyMovement.MovingRight = false;
            _enemyMovement.MovingLeft = true;
        }
        else if (transform.position.x - _nextWaypoint.x < -0.4f)
        {
            _enemyMovement.MovingLeft = false;
            _enemyMovement.MovingRight = true;
        }
        else
        {
            _enemyMovement.MovingLeft = false;
            _enemyMovement.MovingRight = false;
        }

        if (transform.position.y - _nextWaypoint.y < -0.5f)
        {
            _enemyMovement.Jumping = true;
        }
        else
        {
            _enemyMovement.Jumping = false;
        }

        // Use next waypoint if close enough
        if (Vector3.Distance(transform.position, _path.vectorPath[_currentWaypoint]) < _nextWaypointDistance)
        {
            _currentWaypoint++;
        }

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            _path = null;
            _enemyMovement.MovingLeft = false;
            _enemyMovement.MovingRight = false;
        }
    }

    // Generate a path to the left or right.
    void GenerateWalkingPath()
    {   
        // If enemy is outside the max walk radius, move them towards the spawnpoint.
        float _distanceFromEnemyPoint = transform.position.x - EnemyPoint.transform.position.x;
        if (_currentDirection == Direction.Left && _distanceFromEnemyPoint < -_radiusUndetected) _currentDirection = Direction.Right;
        else if (_currentDirection == Direction.Right && _distanceFromEnemyPoint > _radiusUndetected) _currentDirection = Direction.Left;

        // Select a random walk length and create a path in the current direction.
        float _walkLength = Random.Range(_minWalkLength, _maxWalkLength);
        if (_currentDirection == Direction.Right)
        {
            _currentDirection = Direction.Left;
            _seeker.StartPath(transform.position, transform.position + transform.right * _walkLength);
        }
        else
        {
            _currentDirection = Direction.Right;
            _seeker.StartPath(transform.position, transform.position + transform.right * -_walkLength);
        }
    }

    void GenerateAlertPath()
    {
        float _distanceFromEnemyPoint = transform.position.x - EnemyPoint.transform.position.x;
        if (_currentDirection == Direction.Left && _distanceFromEnemyPoint < -_radiusAlert) _currentDirection = Direction.Right;
        else if (_currentDirection == Direction.Right && _distanceFromEnemyPoint > _radiusAlert) _currentDirection = Direction.Left;

        // Select a random walk length and create a path in the current direction.
        float _walkLength = Random.Range(_minWalkLength*2, _maxWalkLength*2);
        if (_currentDirection == Direction.Right)
        {
            _currentDirection = Direction.Left;
            _seeker.StartPath(transform.position, transform.position + transform.right * _walkLength);
        }
        else
        {
            _currentDirection = Direction.Right;
            _seeker.StartPath(transform.position, transform.position + transform.right * -_walkLength);
        }
    }

    void GeneratePursuitPath()
    {
        GraphNode _playerNode = AstarPath.active.GetNearest(_player.transform.position, NNConstraint.Default).node;
        if (_seeker.IsDone()) _seeker.StartPath(transform.position, (Vector3)_playerNode.position);
    }

    void GenerateSearchPath()
    {
        // Get either -1 or 1 as direction.
        int _direction = Random.Range(0, 2) * 2 - 1;
        GraphNode _searchNode = AstarPath.active.GetNearest(new Vector3(_searchPosition.x + (_radiusSearch * _direction), _searchPosition.y), NNConstraint.Default).node;
        _seeker.StartPath(transform.position, (Vector3)_searchNode.position);
    }

    void GenerateHomePath()
    {
        _seeker.StartPath(transform.position, EnemyPoint.transform.position);
    }

    void OnPathComplete(Path _p)
    {
        if(_p.error)
        {
            Debug.LogError(_p.error);
        } else
        {
            _reachedEndOfPath = false;
            _currentWaypoint = 0;
            _path = _p;
            if(_currentPhase == AIPhase.Pursuit) _searchPosition = _path.vectorPath[_path.vectorPath.Count - 1];
        }
    }

    private void CheckLineOfSight()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        if(_currentPhase == AIPhase.Undetected)
        {
            if(_distanceToPlayer < _lineOfSightUndetected)
            {
                // Raycast check for player LOS
                Vector3 _rayStart = transform.position;
                Vector3 _direction = (_player.transform.position - transform.position).normalized;
                RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, _lineOfSightUndetected, _rayColliders);
                if (_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _lineOfSightTimer += Time.deltaTime;
                    if (_lineOfSightTimer > _detectionTimeUndetected) _currentPhase = AIPhase.Pursuit;
                }
                else
                {
                    _lineOfSightTimer = 0f;
                }
            } else
            {
                _lineOfSightTimer = 0f;
            }
        }
        else if(_distanceToPlayer < _lineOfSightAlert)
        {
            Vector3 _rayStart = transform.position;
            Vector3 _direction = (_player.transform.position - transform.position).normalized;
            RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, _lineOfSightAlert, _rayColliders);
            if(_currentPhase == AIPhase.Pursuit)
            {
                if(!(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
                {
                    if (_isCreatingPath)
                    {
                        CancelInvoke();
                        _isCreatingPath = false;
                    }
                    _lineOfSightTimer += Time.deltaTime;
                    if(_lineOfSightTimer > _pursuitTime)
                    {
                        _lineOfSightTimer = 0f;
                        _currentPhase = AIPhase.Searching;
                        _path = null;
                        GenerateSearchPath();
                    }
                } 
                else
                {
                    // Player in range and in line of sight; aim and shoot.
                    _enemyWeapon.AimAt(_player.transform);
                    _enemyWeapon.Shoot();
                    if (!_isCreatingPath)
                    {
                        InvokeRepeating("GeneratePursuitPath", 0f, 0.5f);
                        _isCreatingPath = true;
                    }
                    _lineOfSightTimer = 0f;
                }
            }
            else if(_currentPhase == AIPhase.Searching)
            {
                if(!(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
                {
                    _lineOfSightTimer += Time.deltaTime;
                    if(_lineOfSightTimer > _searchTime)
                    {
                        _lineOfSightTimer = 0f;
                        _currentPhase = AIPhase.Alert;
                        _path = null;
                        GenerateHomePath();
                    }
                } 
                else
                {
                    _currentPhase = AIPhase.Pursuit;
                }
            }
            else if(_currentPhase == AIPhase.Alert)
            {
                if(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _lineOfSightTimer += Time.deltaTime;
                    if(_lineOfSightTimer > _detectionTimeAlert)
                    {
                        _lineOfSightTimer = 0f;
                        _currentPhase = AIPhase.Pursuit;
                    }
                } else
                {
                    _lineOfSightTimer = 0f;
                }
            }
        }
        else
        {
            if(_currentPhase == AIPhase.Pursuit)
            {
                if (_isCreatingPath)
                {
                    CancelInvoke();
                    _isCreatingPath = false;
                }
                _lineOfSightTimer += Time.deltaTime;
                if(_lineOfSightTimer > _pursuitTime)
                {
                    _lineOfSightTimer = 0f;
                    _currentPhase = AIPhase.Searching;
                    _path = null;
                    GenerateSearchPath();
                }
            }
            else if(_currentPhase == AIPhase.Searching)
            {
                _lineOfSightTimer += Time.deltaTime;
                if(_lineOfSightTimer > _searchTime)
                {
                    _lineOfSightTimer = 0f;
                    _currentPhase = AIPhase.Alert;
                    _path = null;
                    GenerateHomePath();
                }
            }
            else if(_currentPhase == AIPhase.Alert)
            {
                _lineOfSightTimer = 0f;
            }
        }
    }

    private void OnDisable()
    {
        _seeker.pathCallback -= OnPathComplete;
    }
}
