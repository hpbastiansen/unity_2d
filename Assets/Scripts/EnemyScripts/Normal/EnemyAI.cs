using UnityEngine;
using Pathfinding;

/// This script contains all the AI for the gunmen enemies. They have 4 distinct phases determining what their action will be: Undetected, Pursuit, Searching, and Alert.
public class EnemyAI : MonoBehaviour
{
    public enum AIPhase { Undetected, Pursuit, Searching, Alert };
    enum Direction { Left, Right };
    public AIPhase CurrentPhase = AIPhase.Undetected;
    private Direction _currentDirection = Direction.Left;

    [HideInInspector] public GameObject EnemyPoint;
    private Seeker _seeker;
    private EnemyMovement _enemyMovement;
    private EnemyWeapon _enemyWeapon;

    private GameObject _player;
    private float _distanceToPlayer;

    private Path _path;
    private bool _reachedEndOfPath = false;
    private int _currentWaypoint = 0;

    private bool _isCreatingPath = false;

    private Vector3 _searchPosition;

    [Header("Pathfinding settings")]
    [SerializeField] private float _nextWaypointDistance;

    [Header("Patrol radius")]
    [SerializeField] private float _radiusUndetected;
    [SerializeField] private float _radiusAlert;
    [SerializeField] private float _radiusSearch;

    [Header("Line of sight settings")]
    [SerializeField] private float _lineOfSightUndetected;
    [SerializeField] private float _lineOfSightAlert;
    [SerializeField] private float _detectionTimeUndetected;
    [SerializeField] private float _detectionTimeAlert;
    [SerializeField] private float _pursuitTime;
    [SerializeField] private float _searchTime;
    [SerializeField] private LayerMask _rayColliders;
    private float _lineOfSightTimer = 0f;
    [SerializeField] private float _maintainRange;

    [Header("Walking length")]
    [SerializeField] private float _minWalkLength;
    [SerializeField] private float _maxWalkLength;

    [Header("Waiting time")]
    private float _waitTimer = 0f;
    private float _waitLength;
    [SerializeField] private float _minWaitLength;
    [SerializeField] private float _maxWaitLength;

    /// Called before the first frame.
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<EnemyWeapon>();
        _seeker = GetComponent<Seeker>();
        _seeker.pathCallback += OnPathComplete;
        _waitLength = Random.Range(_minWaitLength, _maxWaitLength);
        GenerateWalkingPath();
    }

    /// Called every frame.
    /** This method first checks whether the player is in line of sight or not. 
        If the AI phase is 'Undetected', the enemy walks along their created path and waits a random amount of time, then creates a new one. 
        If the AI phase is 'Pursuit', they start running and following their path if they are far away from the player or they lack line of sight.
        If the AI phase is 'Searching', they follow their search path, or generate a new one if it's finished. 
        If the AI phase is 'Alert', they walk back home, then follow the same steps as the 'Undetected' phase. */
    void Update()
    {
        CheckDetection();
        if (CurrentPhase == AIPhase.Undetected)
        {
            _enemyMovement.Running = false;
            if (_reachedEndOfPath)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer > _waitLength)
                {
                    GenerateWalkingPath();
                    _waitLength = Random.Range(_minWaitLength, _maxWaitLength);
                    _waitTimer = 0f;
                }
            }
            if (_path == null) return;

            FollowPath();
        }
        else if (CurrentPhase == AIPhase.Pursuit)
        {
            _enemyMovement.Running = true;
            if (_reachedEndOfPath) return;

            if (_distanceToPlayer > _maintainRange || _lineOfSightTimer > 0)
            {
                FollowPath();
            }
            else
            {
                _enemyMovement.MovingLeft = false;
                _enemyMovement.MovingRight = false;
            }

        }
        else if (CurrentPhase == AIPhase.Searching)
        {
            _enemyMovement.Running = false;
            if (_reachedEndOfPath) GenerateSearchPath();
            if (_path == null) return;

            FollowPath();
        }
        else if (CurrentPhase == AIPhase.Alert)
        {
            _enemyMovement.Running = true;
            if (_reachedEndOfPath)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer > _waitLength)
                {
                    GenerateAlertPath();
                    _waitLength = Random.Range(_minWaitLength, _maxWaitLength);
                    _waitTimer = 0f;
                }
            }
            if (_path == null) return;

            FollowPath();
        }
    }

    /// This method makes the enemy follow the path set.
    /** First, the enemy gets which waypoint on the path they should go towards. 
        Then, they get the direction on the x-axis to the point and move towards it. 
        If the enemy is blocked in front or there is a gap and the waypoint is above them, they start jumping. 
        Lastly, if they are close enough to the waypoint, they set a new waypoint. 
        If the path is complete, they stop moving until a new path is set. */
    private void FollowPath()
    {
        Vector3 _nextWaypoint = _path.vectorPath[_currentWaypoint];
        if (transform.position.x - _nextWaypoint.x > 0.1f)
        {
            _enemyMovement.MovingRight = false;
            _enemyMovement.MovingLeft = true;
        }
        else if (transform.position.x - _nextWaypoint.x < -0.1f)
        {
            _enemyMovement.MovingLeft = false;
            _enemyMovement.MovingRight = true;
        }
        else
        {
            _enemyMovement.MovingLeft = false;
            _enemyMovement.MovingRight = false;
        }

        if (_enemyMovement.BlockedInFront)
        {
            _enemyMovement.Jumping = true;
        }
        else if (_enemyMovement.GapInFront && transform.position.y - _nextWaypoint.y < -0.1f)
        {
            _enemyMovement.Jumping = true;
        }
        else
        {
            _enemyMovement.Jumping = false;
        }

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

    /// This method generates a walking path in the opposite direction of the previous path. If the enemy is too far away from their spawn point, the direction is flipped.
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
            GraphNode _nearestNode = AstarPath.active.GetNearest(transform.position + transform.right * _walkLength).node;
            _seeker.StartPath(transform.position, (Vector3)_nearestNode.position);
        }
        else
        {
            _currentDirection = Direction.Right;
            GraphNode _nearestNode = AstarPath.active.GetNearest(transform.position + transform.right * -_walkLength).node;
            _seeker.StartPath(transform.position, (Vector3)_nearestNode.position);
        }
    }

    /// This method generates a patrol path in the opposite direction of the previous path. If the enemy is too far away from their spawn point, the direction is flipped.
    void GenerateAlertPath()
    {
        float _distanceFromEnemyPoint = transform.position.x - EnemyPoint.transform.position.x;
        if (_currentDirection == Direction.Left && _distanceFromEnemyPoint < -_radiusAlert) _currentDirection = Direction.Right;
        else if (_currentDirection == Direction.Right && _distanceFromEnemyPoint > _radiusAlert) _currentDirection = Direction.Left;

        // Select a random walk length and create a path in the current direction.
        float _walkLength = Random.Range(_minWalkLength, _maxWalkLength);
        if (_currentDirection == Direction.Right)
        {
            _currentDirection = Direction.Left;
            if (_seeker.IsDone()) _seeker.StartPath(transform.position, transform.position + transform.right * _walkLength);
        }
        else
        {
            _currentDirection = Direction.Right;
            if (_seeker.IsDone()) _seeker.StartPath(transform.position, transform.position + transform.right * -_walkLength);
        }
    }

    /// This method gets the closest pathfinding node to the player and generates a path towards it using the Seeker component.
    void GeneratePursuitPath()
    {
        GraphNode _playerNode = AstarPath.active.GetNearest(_player.transform.position).node;
        if (_seeker.IsDone()) _seeker.StartPath(transform.position, (Vector3)_playerNode.position);
    }

    /// This method uses the Seeker component to generate a path from the enemy's position to a random direction from the player's last known position.
    void GenerateSearchPath()
    {
        // Get either -1 or 1 as direction.
        int _direction = Random.Range(0, 2) * 2 - 1;
        GraphNode _searchNode = AstarPath.active.GetNearest(new Vector3(_searchPosition.x + (_radiusSearch * _direction), _searchPosition.y), NNConstraint.Default).node;
        _seeker.StartPath(transform.position, (Vector3)_searchNode.position);
    }

    /// This method uses the Seeker component to generate a path back to the enemy's spawn point.
    void GenerateHomePath()
    {
        _seeker.StartPath(transform.position, EnemyPoint.transform.position);
    }

    /// This method is a callback ran when the Seeker component has finished creating a path.
    /** If the path is valid, set the current path to the new one. Save the last node in the path as the search position if the current phase is 'Pursuit'. */
    void OnPathComplete(Path _p)
    {
        if (!_p.error)
        {
            _reachedEndOfPath = false;
            _currentWaypoint = 0;
            _path = _p;
            if (CurrentPhase == AIPhase.Pursuit) _searchPosition = _path.vectorPath[_path.vectorPath.Count - 1];
        }
    }

    /// This method if fairly long and will be explained in greater details with inline comments.
    /** This method checks the enemy's distance to the player and if they have line of sight to them. */
    private void CheckDetection()
    {
        // Calculate distance from enemy to player.
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        float _deltaX = transform.position.x - _player.transform.position.x;
        // If the player is less than 1 unit away, or less than 5 units away and the enemy is facing them; instantly change phase to pursuit.
        if (_distanceToPlayer < 1f || (_deltaX > 0 && !_enemyMovement.FacingRight && _distanceToPlayer < 5f) || (_deltaX < 0 && _enemyMovement.FacingRight && _distanceToPlayer < 5f))
        {
            CurrentPhase = AIPhase.Pursuit;
        }

        // If the enemy is in the 'Undetected' phase and they are close enough to the player, start raycasting from the enemy to the player.
        // If the enemy has line of sight, begin a timer. If the timer runs over the detection time, change phase to pursuit.
        // If line of sight is broken at any point, reset the timer to 0.
        if (CurrentPhase == AIPhase.Undetected)
        {
            if (_distanceToPlayer < _lineOfSightUndetected)
            {
                Vector3 _rayStart = transform.position;
                Vector3 _direction = (_player.transform.position - transform.position).normalized;
                RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, _lineOfSightUndetected, _rayColliders);
                if (_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _lineOfSightTimer += Time.deltaTime;
                    if (_lineOfSightTimer > _detectionTimeUndetected) CurrentPhase = AIPhase.Pursuit;
                }
                else
                {
                    _lineOfSightTimer = 0f;
                }
            }
            else
            {
                _lineOfSightTimer = 0f;
            }
        }
        // If the enemy is not in the 'Undetected' phase and the player is close enough, start raycasting from the enemy to the player.
        //
        // If the phase is pursuit and there is no line of sight, remove the enemy's target and start increasing a timer.
        // If the line of sight is broken for more than 3 seconds, the enemy stops creating paths to the player. If it is broken for long enough,
        // switch phase to 'Searching' and generate a search path.
        // Else, if there is line of sight, set the player as the shooting target and start creating paths towards them.
        // 
        // If the phase is 'Searching', and there is no line of sight, start increasing a timer.
        // If the timer has ran for long enough, switch phase to 'Alert' and create a path home to the enemy's spawn point.'
        // Else, if there is line of sight, switch phase to 'Pursuit'.
        //
        // If the phase is 'Alert', and there is line of sight, start increasing a timer.
        // If the timer runs for long enough, switch phase to 'Pursuit'.
        else if (_distanceToPlayer < _lineOfSightAlert)
        {
            Vector3 _rayStart = transform.position;
            Vector3 _direction = (_player.transform.position - transform.position).normalized;
            RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, _lineOfSightAlert, _rayColliders);
            if (CurrentPhase == AIPhase.Pursuit)
            {
                if (_raycast.collider == null || !(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
                {
                    _enemyWeapon.Target = null;
                    if (_isCreatingPath && _lineOfSightTimer > 3f)
                    {
                        CancelInvoke();
                        _isCreatingPath = false;
                    }
                    _lineOfSightTimer += Time.deltaTime;
                    if (_lineOfSightTimer > _pursuitTime)
                    {
                        _lineOfSightTimer = 0f;
                        CurrentPhase = AIPhase.Searching;
                        _path = null;
                        GenerateSearchPath();
                    }
                }
                else
                {
                    // Player in range and in line of sight; aim and shoot.
                    _enemyWeapon.Target = _player;

                    if (!_isCreatingPath)
                    {
                        InvokeRepeating("GeneratePursuitPath", 0f, 0.5f);
                        _isCreatingPath = true;
                    }
                    _lineOfSightTimer = 0f;
                }
            }
            else if (CurrentPhase == AIPhase.Searching)
            {
                if (!(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
                {
                    _lineOfSightTimer += Time.deltaTime;
                    if (_lineOfSightTimer > _searchTime)
                    {
                        _lineOfSightTimer = 0f;
                        CurrentPhase = AIPhase.Alert;
                        _path = null;
                        GenerateHomePath();
                    }
                }
                else
                {
                    CurrentPhase = AIPhase.Pursuit;
                }
            }
            else if (CurrentPhase == AIPhase.Alert)
            {
                if (_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _lineOfSightTimer += Time.deltaTime;
                    if (_lineOfSightTimer > _detectionTimeAlert)
                    {
                        _lineOfSightTimer = 0f;
                        CurrentPhase = AIPhase.Pursuit;
                    }
                }
                else
                {
                    _lineOfSightTimer = 0f;
                }
            }
        }
        // If the phase is not 'Undetected' and the player is out of range completely.
        // 
        // If the current phase is 'Pursuit', stop creating paths, remove the enemy's target and increase the timer.
        // If the timer runs long enough, switch phase to 'Searching' and generate a search path.
        //
        // If the current phase is 'Searching', increase the timer.
        // If the timer runs long enough, switch phase to 'Alert' and generate a path home to the enemy's spawn point.
        //
        // If the current phase is 'Alert', reset the timer to 0.
        else
        {
            if (CurrentPhase == AIPhase.Pursuit)
            {
                _enemyWeapon.Target = null;
                if (_isCreatingPath)
                {
                    CancelInvoke();
                    _isCreatingPath = false;
                }
                _lineOfSightTimer += Time.deltaTime;
                if (_lineOfSightTimer > _pursuitTime)
                {
                    _lineOfSightTimer = 0f;
                    CurrentPhase = AIPhase.Searching;
                    _path = null;
                    GenerateSearchPath();
                }
            }
            else if (CurrentPhase == AIPhase.Searching)
            {
                _lineOfSightTimer += Time.deltaTime;
                if (_lineOfSightTimer > _searchTime)
                {
                    _lineOfSightTimer = 0f;
                    CurrentPhase = AIPhase.Alert;
                    _path = null;
                    GenerateHomePath();
                }
            }
            else if (CurrentPhase == AIPhase.Alert)
            {
                _lineOfSightTimer = 0f;
            }
        }
    }

    /// This method is called once the gameobject it's attached to is disabled.
    /** Remove the callback from the seeker component on path completion. */
    private void OnDisable()
    {
        _seeker.pathCallback -= OnPathComplete;
    }
}
