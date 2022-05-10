// https://www.youtube.com/watch?v=9hTnlp9_wX8 Creating the worm body
// https://www.youtube.com/watch?v=BfP0KyOxVWs Collision for line renderer

using System.Collections.Generic;
using UnityEngine;

/// The WormBody script handles the drawing and collision of the worm enemy's body.

public class WormBody : MonoBehaviour
{
    private WormCollision _wormCollision;
    private LineRenderer _lineRend;
    [SerializeField] private GameObject _bodySegment;
    [SerializeField] private Sprite[] _bodySprites;

    [Header("Body Length")]
    [SerializeField] private int _minLength = 4;
    [SerializeField] private int _maxLength = 8;
    private int _length;
    
    [Header("Body Segments")]
    [SerializeField] private Transform _targetDir;
    [SerializeField] private float _targetDist;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private float _trailSpeed;
    [SerializeField] private Transform _tailEnd;
    private List<Transform> _bodySegments;

    [Header("Wiggle")]
    [SerializeField] private Transform _wiggleDir;
    [SerializeField] private float _wiggleSpeed;
    [SerializeField] private float _wiggleMagnitude;

    private Vector3[] _segmentPoses;
    private Vector3[] _segmentV;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we find the necessary components, choose the worm's length and prepare the Line Renderer.*/
    private void Start()
    {
        _wormCollision = transform.root.GetComponent<WormCollision>();
        _lineRend = GetComponent<LineRenderer>();
        _length = (int)Mathf.Floor(Random.Range(_minLength, _maxLength));
        _lineRend.positionCount = _length;
        _segmentPoses = new Vector3[_length];
        _segmentV = new Vector3[_length];
        CreateBodySprites();
        ResetPos();
    }

    /// Update is called every frame.
    /** The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
        This means that is a game run on higher frames per second the update function will also be called more often. */
    /*! Note: This update fuction is fairly long, and more documentation will be written "inline" in the code itself. */
    /*! The update function moves the body segments sequentially, creating a natural flowing movement. We also draw the Polygon Collider around the segments. */
    private void Update()
    {
        // Changes the wiggle rotation based on the current time passed into a sine function.
        _wiggleDir.localRotation = Quaternion.Euler(0, 0, (Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleMagnitude) + 270);

        // Sets the position of the first body segment manually.
        _segmentPoses[0] = _targetDir.position;

        // Moves the other body segments smoothly behind the first one.
        for(int _i = 1; _i < _segmentPoses.Length; _i++)
        {
            _segmentPoses[_i] = Vector3.SmoothDamp(_segmentPoses[_i], _segmentPoses[_i - 1] + _targetDir.right * _targetDist, ref _segmentV[_i], _smoothSpeed + _i / _trailSpeed);
        }
        _lineRend.SetPositions(_segmentPoses);

        // Set the body and tail sprite to follow the body line renderer.
        SetBodySprites();
        SetTailSprite();
        
        // Set the polygon collider to match the worm's visuals.
        _wormCollision.SetCollision(_segmentPoses, _length, _lineRend.startWidth);
    }

    /// Resets the positions of the Line Renderer so they do not start at (0, 0, 0) in world space. Used only at instantiation.
    private void ResetPos()
    {
        _segmentPoses[0] = _targetDir.position;
        for(int _i = 1; _i < _length; _i++)
        {
            _segmentPoses[_i] = _segmentPoses[_i - 1] + _targetDir.right * _targetDist;
        }
        _lineRend.SetPositions(_segmentPoses);
    }

    /// Set the position of the body sprites to be on top of each line renderer point and rotate them to face the correct orientation.
    private void SetBodySprites()
    {
        for(int _i = 1; _i < _length-1; _i++)
        {
            _bodySegments[_i].position = _segmentPoses[_i];
            Vector2 _segmentDirection = _segmentPoses[_i - 1] - _bodySegments[_i].position;
            float _angle = Vector2.Angle(transform.root.right, _segmentDirection);
            _bodySegments[_i].localEulerAngles = new Vector3(0, 0, _angle - 90f);
        }
    }

    /// Set the tail sprite to be at the last segment of the worm.
    private void SetTailSprite()
    {
        _tailEnd.position = _segmentPoses[_segmentPoses.Length - 1];
        Vector2 _tailDirection = _segmentPoses[_segmentPoses.Length - 2] - _tailEnd.position;
        float _angle = Vector2.Angle(transform.root.right, _tailDirection);
        _tailEnd.localEulerAngles = new Vector3(0, 0, _angle - 90f);
    }

    /// Create a body sprite for each segment of the worm's body.
    private void CreateBodySprites()
    {
        _bodySegments = new List<Transform>();
        for (int _i = 0; _i < _length; _i++)
        {
            GameObject _segment = Instantiate(_bodySegment, transform);
            Sprite _randomSprite = _bodySprites[Random.Range(0, _bodySprites.Length)];
            _segment.GetComponent<SpriteRenderer>().sprite = _randomSprite;
            _bodySegments.Add(_segment.transform);
        }
    }
}
