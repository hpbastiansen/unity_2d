using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI_Hunter : MonoBehaviour
{
    private Seeker _seeker;
    private EnemyMovement _enemyMovement;
    private Weapon_Enemy _enemyWeapon;
    private GameObject _player;
    private float _distanceToPlayer;
    private Path _path;
    private bool _reachedEndOfPath = false;
    private int _currentWaypoint = 0;
    private bool _lineOfSight = false;

    [SerializeField] private float _nextWaypointDistance;
    [SerializeField] private float _maintainRange;
    [SerializeField] private LayerMask _rayColliders;

    // Start is called before the first frame update
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyMovement.Running = true;
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<Weapon_Enemy>();
        _seeker = GetComponent<Seeker>();
        _seeker.pathCallback += OnPathComplete;
        InvokeRepeating("GeneratePursuitPath", 0f, 0.5f);
    }

    

    // Update is called once per frame
    private void Update()
    {
        CheckLineOfSight();
        if (_reachedEndOfPath) return;
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

    private void GeneratePursuitPath()
    {
        GraphNode _playerNode = AstarPath.active.GetNearest(_player.transform.position, NNConstraint.Default).node;
        if (_seeker.IsDone()) _seeker.StartPath(transform.position, (Vector3)_playerNode.position);
    }

    private void OnPathComplete(Path _p)
    {
        if (!_p.error)
        {
            _reachedEndOfPath = false;
            _currentWaypoint = 0;
            _path = _p;
        }
    }

    private void CheckLineOfSight()
    {
        Vector3 _rayStart = transform.position;
        Vector3 _direction = (_player.transform.position - transform.position).normalized;
        RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, 30f, _rayColliders);
        if(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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
