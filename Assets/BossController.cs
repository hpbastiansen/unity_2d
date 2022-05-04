using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [System.Serializable]
    public class SetPieceList
    {
        public GameObject[] Segments;
    }

    [SerializeField] private Transform _knockOffPoint;
    [SerializeField] private Transform _jumpUpPoint;
    [SerializeField] private SetPieceList[] _setPieces;
    [SerializeField] private GameObject _bossSegmentBase;
    [SerializeField] private GameObject _bossSegmentBase2;
    [SerializeField] private GameObject _bossSegmentSpike1;
    [SerializeField] private GameObject _bossSegmentSpike2;
    [SerializeField] private GameObject _bossSegmentWormspawn;
    [SerializeField] private GameObject _bossSegmentWeakpoint;
    [SerializeField] private GameObject _bossSegmentSectionDone;
    private List<GameObject> _bossSegments = new List<GameObject>();
    private List<GameObject> _bossSegmentsBuffer = new List<GameObject>();
    public bool OnWorm = false;
    [SerializeField] private MovingFloor _floor;
    [SerializeField] private MovingBackground _background;
    [SerializeField] private BossMovement _bossMovement;
    [SerializeField] private Transform _boss;
    private GameObject _player;
    private float _wormSpeed;
    private float _playerMoved = 0;
    private float _bossMoved = 0;
    private float _previousPlayerPosition;
    private float _previousBossPosition;
    private float _segmentLength = 7.8125f;
    [SerializeField] private Transform _followPlayer;
    private bool _offset = false;
    private bool _shouldFollow = false;
    public int SectionsDone = 0;
    private BossCutsceneManager _cutsceneManager;
    public bool ReadyForNextSection = false;
    private bool _nextIsWeakpoint = false;
    public int MaxHealth;
    public int Health;
    [SerializeField] private int _damageMin;
    [SerializeField] private int _damageMax;
    public bool IsPlayerDebuffed;
    private Stage5Manager _stage5Manager;
    [SerializeField] private Transform _movementBoundary;

    public void OnKnockedOff()
    {
        PlayerOffBoss();
    }

    public void OnJumpedOn()
    {
        PlayerOnBoss();
    }

    private void Start()
    {
        Health = MaxHealth;
        _stage5Manager = Object.FindObjectOfType<Stage5Manager>();
        _cutsceneManager = GameObject.Find("CutsceneManager").GetComponent<BossCutsceneManager>();
        _player = GameObject.Find("Main_Character");
        foreach (Transform _segment in GameObject.Find("Segments").transform)
        {
            _bossSegments.Add(_segment.gameObject);
        }
    }

    public void Trigger()
    {
        _previousPlayerPosition = _player.transform.position.x;
        _floor.Triggered = true;
        _background.Triggered = true;
        _wormSpeed = 10f;
        PlayerOnBoss();
    }

    private void PlayerOnBoss()
    {
        _background.Speed = _wormSpeed;
        _floor.Speed = _wormSpeed;
        _shouldFollow = true;
        _bossMovement.Speed = 0;
        OnWorm = true;
        ReadyForNextSection = true;
    }

    private void PlayerOffBoss()
    {
        _background.Speed = 0;
        _floor.Speed = 0;
        _shouldFollow = false;
        _bossMovement.Speed = _wormSpeed;
        OnWorm = false;

        if (Health > 0) Invoke("ExternalPuzzleDone", 5f);
    }

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

    private void AddWeakPointSection()
    {
        _bossSegmentsBuffer.Add(_bossSegmentWeakpoint);
        ReadyForNextSection = false;
        _nextIsWeakpoint = false;
    }

    private void PopulateBuffer()
    {
        while (_bossSegmentsBuffer.Count < 2)
        {
            int randomBase = Random.Range(0, 20);
            if (randomBase == 0) _bossSegmentsBuffer.Add(_bossSegmentBase2);
            else if (randomBase < 4) _bossSegmentsBuffer.Add(_bossSegmentWormspawn);
            else if (randomBase < 8) _bossSegmentsBuffer.Add(_bossSegmentSpike1);
            else if (randomBase < 12) _bossSegmentsBuffer.Add(_bossSegmentSpike2);
            else _bossSegmentsBuffer.Add(_bossSegmentBase);
        }
    }

    private void ExternalPuzzleDone()
    {
        _cutsceneManager.JumpOnBoss();
    }

    // Add the next buffered segment to the end of the worm and remove the first segment.
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

    private void Update()
    {
        if (Health <= 0) return;
        PopulateBuffer();
        IsPlayerDebuffed = _stage5Manager.IsDebuffed;
        if(_movementBoundary.position.x - _player.transform.position.x > 0)
        {
            _movementBoundary.position = new Vector3(_movementBoundary.position.x - _segmentLength, _movementBoundary.position.y, _movementBoundary.position.z);
        }

        _knockOffPoint.position = new Vector3(_player.transform.position.x + 3, _knockOffPoint.position.y, _knockOffPoint.position.z);
        _jumpUpPoint.position = new Vector3(_player.transform.position.x + 3, _jumpUpPoint.position.y, _jumpUpPoint.position.z);
        if (_shouldFollow) _followPlayer.position = new Vector3(_player.transform.position.x, _followPlayer.position.y, _followPlayer.position.z);
        if (!OnWorm)
        {
            _bossMoved += _boss.position.x - _previousBossPosition;
            if(_bossMoved >= _segmentLength)
            {
                NewSegmentBack();
                _bossMoved -= _segmentLength;
            }

            _previousBossPosition = _boss.position.x;
        }
        else
        {
            if (SectionsDone >= 3)
            {
                _nextIsWeakpoint = true;
                SectionsDone = 0;
            }

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

            _playerMoved += _player.transform.position.x - _previousPlayerPosition;

            if (_playerMoved >= _segmentLength)
            {
                NewSegment();
                _playerMoved -= _segmentLength;
            }

            _previousPlayerPosition = _player.transform.position.x;
        }
    }
}
