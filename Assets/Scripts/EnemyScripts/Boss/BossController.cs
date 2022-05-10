using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// This script handles most of the boss logic. It is accessed through various other scripts.
public class BossController : MonoBehaviour
{
    // Custom class to allow setting up arrays of arrays of gameobject in the Unity inspector. Used for creating a specific section of segments that should spawn after another.
    [System.Serializable]
    public class SetPieceList
    {
        public GameObject[] Segments;
    }
    
    [Header("Boss segment prefabs")]
    [SerializeField] private GameObject _bossSegmentBase;
    [SerializeField] private GameObject _bossSegmentBase2;
    [SerializeField] private GameObject _bossSegmentSpike1;
    [SerializeField] private GameObject _bossSegmentSpike2;
    [SerializeField] private GameObject _bossSegmentSpike3Grapple;
    [SerializeField] private GameObject _bossSegmentWormspawn;
    [SerializeField] private GameObject _bossSegmentWeakpoint;
    [SerializeField] private GameObject _bossSegmentSectionDone;
    [SerializeField] private GameObject _bossSegmentEnemySpawn;
    [SerializeField] private SetPieceList[] _setPieces;

    [Header("Moving pieces")]
    [SerializeField] private MovingFloor _floor;
    [SerializeField] private MovingBackground _background;
    [SerializeField] private BossMovement _bossMovement;
    [SerializeField] private Transform _boss;
    [SerializeField] private Transform _followPlayer;
    [SerializeField] private Transform _movementBoundary;
    [SerializeField] private BoxCollider2D _movementBoundaryRightCollider;
    [SerializeField] private BoxCollider2D _movementBoundaryLeftCollider;
    private float _wormSpeed;
    private float _playerMoved = 0;
    private float _bossMoved = 0;
    private float _previousPlayerPosition;
    private float _previousBossPosition;
    private float _segmentLength = 7.8125f;
    private bool _offset = false;
    private bool _shouldFollow = false;

    [Header("Cutscene objects")]
    [SerializeField] private Transform _knockOffPoint;
    [SerializeField] private Transform _jumpUpPoint;
    private BossCutsceneManager _cutsceneManager;

    [Header("Damage")]
    [SerializeField] private int _damageMin;
    [SerializeField] private int _damageMax;
    public int MaxHealth;
    public int Health;

    [Header("UI")]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Text _healthText;

    // Various logic variables.
    [HideInInspector] public bool OnWorm = false;
    [HideInInspector] public bool ReadyForNextSection = false;
    [HideInInspector] public int SectionsDone = 0;
    private bool _nextIsWeakpoint = false;

    // Other variables
    [HideInInspector] public bool IsPlayerDebuffed;
    private List<GameObject> _bossSegments = new List<GameObject>();
    private List<GameObject> _bossSegmentsBuffer = new List<GameObject>();
    private GameObject _player;
    private Stage5Manager _stage5Manager;
    
    /// Access point for the cutscene manager. Called when the player is done falling off the boss.
    public void OnKnockedOff()
    {
        PlayerOffBoss();
    }

    /// Access point for the cutscene manager. Called when the player is done jumping on the boss.
    public void OnJumpedOn()
    {
        PlayerOnBoss();
    }

    /// Starts the boss fight.
    /** Triggers the moving floor and walls to simulate the boss' movement. */
    public void Trigger()
    {
        _previousPlayerPosition = _player.transform.position.x;
        _floor.Triggered = true;
        _background.Triggered = true;
        _wormSpeed = 10f;
        PlayerOnBoss();
    }

    /// Reduces the boss' health.
    /** Deals a random amount of damage between _damageMin and _damageMax. Damage is doubled if player successfully removed their debuff. If boss is not dead, knock the player off. */
    public void WeakpointDestroyed()
    {
        int _damage = Random.Range(_damageMin, _damageMax + 1) * (IsPlayerDebuffed ? 1 : 2);
        Health -= _damage;

        if (Health <= 0)
        {
            _cutsceneManager.BossDeath();
        }
        else
        {
            _cutsceneManager.KnockOffBoss();
        }
    }

    /// Called before the first frame.
    /** Sets up health and various gameobjects. All the boss segments already in the scene is added to the internal list. */
    private void Start()
    {
        Health = MaxHealth;
        _stage5Manager = FindObjectOfType<Stage5Manager>();
        _cutsceneManager = GameObject.Find("CutsceneManager").GetComponent<BossCutsceneManager>();
        _player = GameObject.Find("Main_Character");
        foreach (Transform _segment in GameObject.Find("Segments").transform)
        {
            _bossSegments.Add(_segment.gameObject);
        }
    }

    /// Called every frame.
    /** Checks for player debuff, populates the buffer, ensures the worm spawns segments correctly to look seamless and moves various gameobjects that should follow the player or boss. */
    private void Update()
    {
        if (Health <= 0) return;
        PopulateBuffer();
        IsPlayerDebuffed = _stage5Manager.IsDebuffed;

        // If the player manages to get behind the movement boundary, it is moved back to not trap the player.
        if (_movementBoundary.position.x - _player.transform.position.x > 0)
        {
            _movementBoundary.position = new Vector3(_movementBoundary.position.x - _segmentLength, _movementBoundary.position.y, _movementBoundary.position.z);
        }

        // Moves the knock off point and jump up point with the player. If the player is on the worm, moves the floor and walls behind them too.
        _knockOffPoint.position = new Vector3(_player.transform.position.x + 3, _knockOffPoint.position.y, _knockOffPoint.position.z);
        _jumpUpPoint.position = new Vector3(_player.transform.position.x + 3, _jumpUpPoint.position.y, _jumpUpPoint.position.z);
        if (_shouldFollow) _followPlayer.position = new Vector3(_player.transform.position.x, _followPlayer.position.y, _followPlayer.position.z);

        // If the player is off the worm, the worm moves forwards, spawning new segments at the back after moving the length of one segment.
        // If the player is on the worm, the worm stands still, spawning new segments at the front after the player has moved the length of one segment.
        // Also disables the left collider if player is moving to the right to let enemies pass it.
        if (!OnWorm)
        {
            _bossMoved += _boss.position.x - _previousBossPosition;
            if (_bossMoved >= _segmentLength)
            {
                NewSegmentBack();
                _bossMoved -= _segmentLength;
            }

            _previousBossPosition = _boss.position.x;
        }
        else
        {
            _playerMoved += _player.transform.position.x - _previousPlayerPosition;
            if (_playerMoved >= _segmentLength)
            {
                NewSegment();
                _playerMoved -= _segmentLength;
            }

            if (_player.transform.position.x > _previousPlayerPosition) _movementBoundaryLeftCollider.enabled = false;
            else _movementBoundaryLeftCollider.enabled = true;

            _previousPlayerPosition = _player.transform.position.x;
        }

        // If the player has gone through three sections, mark the next section as weakpoint.
        if (SectionsDone >= 3)
        {
            _nextIsWeakpoint = true;
            SectionsDone = 0;
        }

        // Spawn a new section if the player is ready.
        if (ReadyForNextSection)
        {
            if (_nextIsWeakpoint)
            {
                AddWeakPointSection();
            }
            else
            {
                AddRandomSection();
            }
        }

        // Updates the boss' health bar.
        _healthBar.maxValue = MaxHealth;
        _healthBar.value = Health;
        _healthText.text = (Health.ToString("F0") + " / " + MaxHealth.ToString("F0")).ToString();
    }

    /// Called when the player has jumped on the boss. Stops the moving boss, starts the moving floor and background and makes them follow the player.
    private void PlayerOnBoss()
    {
        _background.Speed = _wormSpeed;
        _floor.Speed = _wormSpeed;
        _shouldFollow = true;
        _bossMovement.Speed = 0;
        OnWorm = true;
        ReadyForNextSection = true;
        _movementBoundaryRightCollider.enabled = false;
    }

    /// Called when the player has been knocked off the boss. Stops the moving floor and background, starts the boss movement. They have 5 seconds to complete the external puzzle.
    private void PlayerOffBoss()
    {
        _background.Speed = 0;
        _floor.Speed = 0;
        _shouldFollow = false;
        _bossMovement.Speed = _wormSpeed;
        OnWorm = false;
        _movementBoundaryRightCollider.enabled = true;

        if (Health > 0) Invoke("ExternalPuzzleDone", 5f);
    }

    /// Adds a random setpiece of boss segments to the buffer as defined in the _setPieces variable.
    private void AddRandomSection()
    {
        GameObject[] setPiece = _setPieces[Random.Range(0, _setPieces.Length)].Segments;
        foreach (GameObject segment in setPiece)
        {
            _bossSegmentsBuffer.Add(segment);
        }
        _bossSegmentsBuffer.Add(_bossSegmentSectionDone);
        ReadyForNextSection = false;
    }

    /// Adds a weakpoint segment to the buffer.
    private void AddWeakPointSection()
    {
        _bossSegmentsBuffer.Add(_bossSegmentWeakpoint);
        ReadyForNextSection = false;
        _nextIsWeakpoint = false;
    }

    /// Adds a random segment to the segment buffer until at least two segments is in it. The chance to get a specific segment is weighted.
    private void PopulateBuffer()
    {
        while (_bossSegmentsBuffer.Count < 2)
        {
            int randomSegment = Random.Range(0, 20);

            if (randomSegment <= 4) _bossSegmentsBuffer.Add(_bossSegmentEnemySpawn);
            else if (randomSegment <= 8) _bossSegmentsBuffer.Add(_bossSegmentWormspawn);
            else if (randomSegment <= 11) _bossSegmentsBuffer.Add(_bossSegmentSpike1);
            else if (randomSegment <= 14) _bossSegmentsBuffer.Add(_bossSegmentSpike2);
            else if (randomSegment <= 16) _bossSegmentsBuffer.Add(_bossSegmentSpike3Grapple);
            else if (randomSegment <= 17) _bossSegmentsBuffer.Add(_bossSegmentBase2);
            else _bossSegmentsBuffer.Add(_bossSegmentBase);
        }
    }

    /// Called after five seconds of being off the boss. Starts the cutscene of jumping on the boss.
    private void ExternalPuzzleDone()
    {
        _cutsceneManager.JumpOnBoss();
    }

    /// Add the next buffered segment to the front (end) of the worm and remove the first segment.
    private void NewSegment()
    {
        Vector3 lastSegmentPosition = _bossSegments[_bossSegments.Count - 1].transform.position;
        Vector3 newSegmentPosition = new Vector3(lastSegmentPosition.x + _segmentLength, lastSegmentPosition.y + (_offset ? -0.2f : 0.2f), lastSegmentPosition.z);
        GameObject newSegment = Instantiate(_bossSegmentsBuffer[0], newSegmentPosition, Quaternion.identity, GameObject.Find("Segments").transform);
        _bossSegments.Add(newSegment);
        _bossSegmentsBuffer.RemoveAt(0);
        Destroy(_bossSegments[0]);
        _bossSegments.RemoveAt(0);
        _offset = !_offset;
        _movementBoundary.position = new Vector3(_movementBoundary.position.x + _segmentLength, _movementBoundary.position.y, _movementBoundary.position.z);
    }

    /// Add the next buffered segment to the back (start) of the worm and remove the last segment.
    private void NewSegmentBack()
    {
        Vector3 firstSegmentPosition = _bossSegments[0].transform.position;
        Vector3 newSegmentPosition = new Vector3(firstSegmentPosition.x - _segmentLength, firstSegmentPosition.y + (_offset ? -0.2f : 0.2f), firstSegmentPosition.z);
        GameObject newSegment = Instantiate(_bossSegmentsBuffer[0], newSegmentPosition, Quaternion.identity, GameObject.Find("Segments").transform);
        _bossSegments.Insert(0, newSegment);
        _bossSegmentsBuffer.RemoveAt(0);
        Destroy(_bossSegments[_bossSegments.Count - 1]);
        _bossSegments.RemoveAt(_bossSegments.Count - 1);
        _offset = !_offset;
        _movementBoundary.position = new Vector3(_movementBoundary.position.x - _segmentLength, _movementBoundary.position.y, _movementBoundary.position.z);
    }
}
