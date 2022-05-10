using UnityEngine;
using Pathfinding;

/// This is a version of the EnemyAI script with only the 'Pursuit' AI phase active. It always hunts down the player, regardless of line of sight and distance.
public class EnemyAI_Hunter : MonoBehaviour
{
    private Seeker _seeker;
    private EnemyMovement _enemyMovement;
    private EnemyWeapon _enemyWeapon;
    private GameObject _player;
    private float _distanceToPlayer;
    private Path _path;
    private bool _reachedEndOfPath = false;
    private int _currentWaypoint = 0;
    private bool _lineOfSight = false;

    [SerializeField] private float _nextWaypointDistance;
    [SerializeField] private float _maintainRange;
    [SerializeField] private LayerMask _rayColliders;

    /// Called before the first frame.
    /** In the Start method we set necessary components and start repeatedly calling the GeneratePursuitPath method. */
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyMovement.Running = true;
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<EnemyWeapon>();
        _seeker = GetComponent<Seeker>();
        _seeker.pathCallback += OnPathComplete;
        InvokeRepeating("GeneratePursuitPath", 0f, 0.5f);
    }

    /// Called every frame.
    /** In the Update method we calculate the distance to the player and check line of sight. If the Enemy has a path set, they will follow it. 
        If the distance between the enemy and the player is lower than their 'Maintain Range' and they have line of sight, they stop moving. */
    private void Update()
    {
        _distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
        CheckLineOfSight();
        if (_reachedEndOfPath) return;
        if (_path == null) return;
        if (_distanceToPlayer > _maintainRange || !_lineOfSight)
        {
            FollowPath();
        }
        else
        {
            _enemyMovement.MovingLeft = false;
            _enemyMovement.MovingRight = false;
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

    /// This method gets the closest pathfinding node to the player and generates a path towards it using the Seeker component.
    private void GeneratePursuitPath()
    {
        GraphNode _playerNode = AstarPath.active.GetNearest(_player.transform.position, NNConstraint.Default).node;
        if (_seeker.IsDone()) _seeker.StartPath(transform.position, (Vector3)_playerNode.position);
    }

    /// This method is called when the Seeker component has finished plotting a path.
    /** In the method, we set the generated path as our new path, if no error occured. */
    private void OnPathComplete(Path _p)
    {
        if (!_p.error)
        {
            _reachedEndOfPath = false;
            _currentWaypoint = 0;
            _path = _p;
        }
    }

    /// This method checks if the enemy can see the player within 30 units.
    /** The method casts a ray from the enemy to the player. If the ray hits the player, they set the player as their target for shooting. */
    private void CheckLineOfSight()
    {
        Vector3 _rayStart = transform.position;
        Vector3 _direction = (_player.transform.position - transform.position).normalized;
        RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, 30f, _rayColliders);
        if(_raycast.collider == null)
        {
            _lineOfSight = false;
            _enemyWeapon.Target = null;
        }
        else if(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _lineOfSight = true;
            _enemyWeapon.Target = _player;
        }
        else
        {
            _lineOfSight = false;
            _enemyWeapon.Target = null;
        }
    }
}
